using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;
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
            var type = App.Assembly
                .ExportedTypes
                .SingleOrDefault(x => 
                    x.Name == "Command" 
                    && x.DeclaringType != null 
                    && x.DeclaringType.Name == Name);

            if (type == null )
            {
                Console2.RedLine($"Type {Name}.Command not found in assembly {App.Assembly.FullName}");
                return;
            }

            var instance = Activator.CreateInstance(type);

            if (instance is RootCommand rootCommand)
            {
                rootCommand.TreatUnmatchedTokensAsErrors = false;
                
                var binder = new ModelBinder(instance.GetType())
                {
                    EnforceExplicitBinding = false
                };
                
                // excludes from args 'invoke FeatureName'
                var result = rootCommand.Parse(_argsConfig.CliArgs[2..]);
                var invocationContext = new InvocationContext(result);
                var bindingContext = invocationContext.BindingContext;

                binder.UpdateInstance(instance, bindingContext);
            }
            
            if (instance is IInvokable request)
            {
                await _app.ScopedSendAsync(request);

                Console2.GreenLine("Done");

                return;
            }

            Console2.RedLine($"Type {Name}.Command is not a IInvokable");
        }
    }
}