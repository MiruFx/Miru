using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Foundation.Bootstrap;

namespace Miru.Pipeline;

public class InvokeConsolable : Consolable
{
    public InvokeConsolable() : base("invoke", "Invokes a command")
    {
        Add(new Argument<string>("name"));
    }
            
    public class ConsolableHandler : IConsolableHandler
    {
        public string Name { get; set; }

        private readonly IMiruApp _app;
        private readonly ArgsConfiguration _argsConfig;

        public ConsolableHandler(IMiruApp app, ArgsConfiguration argsConfig)
        {
            _app = app;
            _argsConfig = argsConfig;
        }
                
        public async Task Execute()
        {
            var type = SearchTypeByInvokableName(Name);

            if (type is null)
            {
                Console2.RedLine($"Type {Name}.Command not found in assembly {App.Assembly.FullName}");
                return;
            }

            var invokableRequest = BuildInvokableRequest(type, _argsConfig.CliArgs[2..]);

            if (invokableRequest is IInvokable request)
            {
                await _app.ScopedSendAsync(request);

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
            
            return new YamlDotNet.Serialization.Deserializer().Deserialize(yml, type);
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
}