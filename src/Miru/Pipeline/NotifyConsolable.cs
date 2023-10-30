using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Miru.Consolables;
using Miru.Foundation.Bootstrap;

namespace Miru.Pipeline;

public class NotifyConsolable : Consolable
{
    public NotifyConsolable() : base("notify", "Notify an event enqueueing it")
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
            var type = SearchTypeByNotificationName(Name);

            if (type is null)
            {
                Console2.RedLine($"Type {Name}.Notification not found in assembly {App.Assembly.FullName}");
                return;
            }

            var invokableRequest = BuildNotificationRequest(type, _argsConfig.CliArgs[2..]);

            if (invokableRequest is INotification notification)
            {
                await _app.ScopedPublishAsync(notification, default);

                Console2.GreenLine("Done");

                return;
            }

            Console2.RedLine($"Type {Name}.Notification is not a INotification");
        }

        /// <summary>
        /// Not very proud of this method but is very handy
        /// Builds a yml string and use YamlDotNet to create an instance of type
        /// </summary>
        private object BuildNotificationRequest(Type type, string[] cliArgs)
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

        private Type SearchTypeByNotificationName(string invokableName)
        {
            return App.Assembly
                .ExportedTypes
                .SingleOrDefault(x => 
                    x.Name == "Notification" 
                    && x.DeclaringType != null 
                    && x.DeclaringType.Name == invokableName);
        }
    }
}