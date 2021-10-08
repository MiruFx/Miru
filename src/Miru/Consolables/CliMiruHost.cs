using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using Miru.Foundation.Bootstrap;
using Miru.Foundation.Hosting;

namespace Miru.Consolables
{
    public class CliMiruHost : ICliMiruHost
    {
        private readonly IEnumerable<Consolable> _consolable;
        private readonly ArgsConfiguration _argsConfig;
        private readonly IMiruApp _app;

        public CliMiruHost(IEnumerable<Consolable> commands, ArgsConfiguration argsConfig, IMiruApp app)
        {
            _consolable = commands;
            _argsConfig = argsConfig;
            _app = app;
        }

        public async Task RunAsync(CancellationToken token = default)
        {
            await RunAsync(_argsConfig.CliArgs.Join(" "));
        }

        public async Task RunAsync(string args)
        {
            var rootCommand = new RootCommand
            {
                Name = "miru"
            };

            foreach (var consolable in _consolable)
            {
                rootCommand.AddCommand(consolable);
            }

            var result = rootCommand.Parse(args);
            
            if (result.CommandResult.Command is not RootCommand && 
                result.CommandResult.Command is Command command)
            {
                var handlerType = command.GetType().Assembly.GetType($"{command.GetType().FullName}+ConsolableHandler");
                
                await _app.WithScope(async scope =>
                {
                    var handler = (IConsolableHandler) scope.Get(handlerType);
                    
                    command.Handler = CommandHandler.Create(handlerType.GetMethod(nameof(IConsolableHandler.Execute)), handler);
                
                    await result.InvokeAsync();
                });
            }
            else
            {
                await rootCommand.InvokeAsync(args);
            }
        }
    }
}