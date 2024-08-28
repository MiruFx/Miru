using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Miru.Consolables;
using Miru.Foundation.Bootstrap;
using Miru.Mvc;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Miru.Pipeline;

public class InvokeConsolable : Consolable
{
    public InvokeConsolable() : base("invoke", "Invokes a command")
    {
        Add(new Argument<string>("name"));
    }
            
    public class ConsolableHandler(IMiruApp app, ArgsConfiguration argsConfig) : IConsolableHandler
    {
        public string Name { get; set; }

        public async Task Execute()
        {
            var type = SearchTypeByInvokableName(Name);

            if (type is null)
            {
                Console2.RedLine($"Type {Name}.Command not found in assembly {App.Assembly.FullName}");
                return;
            }

            var invokableRequest = BuildInvokableRequest(type, argsConfig.CliArgs[2..]);

            if (invokableRequest is IInvokable request)
            {
                await app.ScopedSendAsync(request);

                Console2.GreenLine("Done");

                return;
            }

            Console2.RedLine($"Type {Name}.Command is not a IInvokable");
        }

        /// <summary>
        /// Not very proud of this method but is very handy
        /// Builds a yml string and use YamlDotNet to create an instance of type
        /// </summary>
        private object BuildInvokableRequest(Type type, string[] cliArgs)
        {
            var rootCommand = CliMiruHost.CreateRootCommand();
            var parseResult = rootCommand.Parse(cliArgs);

            if (cliArgs.Length == 0)
                return Activator.CreateInstance(type);
            
            var yml = parseResult.Tokens
                .Select((x, i) => new { Index = i, Item = x })
                .GroupBy(x => x.Index / 2)
                .Select(x => $"{x.At(0).Item.Value[2..]}: {x.At(1).Item.Value}")
                .Join(Environment.NewLine);

            var deserializer = new DeserializerBuilder()
                .WithTypeConverter(new SmartEnumTypeConverter())
                .IgnoreUnmatchedProperties() // don't throw an exception if there are unknown properties
                .Build();
            
            return deserializer.Deserialize(yml, type);
        }

        private Type SearchTypeByInvokableName(string invokableName)
        {
            return App.Assembly
                .ExportedTypes
                .SingleOrDefault(x => 
                    x.Name == "Command" 
                    && x.DeclaringType != null 
                    && x.DeclaringType.Name == invokableName);
        }
    }
    
    public class SmartEnumTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type.Implements<ISmartEnum>();
        }

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            var value = parser.Consume<Scalar>().Value;
            
            Type baseSmartEnumType = TypeUtil.GetTypeFromGenericType(type, typeof(SmartEnum<,>));
            foreach (MethodInfo methodInfo in baseSmartEnumType.GetMethods())
            {
                if (methodInfo.Name == "FromValue")
                {
                    ParameterInfo[] methodsParams = methodInfo.GetParameters();
                    if (methodsParams.Length == 1)
                    {
                        if (methodsParams[0].ParameterType == typeof(int))
                        {
                            return methodInfo.Invoke(null, new object[] { value.ToInt() });
                        }
                    }
                }
            }
            
            throw new InvalidOperationException("Could not parse a SmartEnum");
        }

        public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}