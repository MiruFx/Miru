using System.Threading;
using System.Threading.Tasks;
using Baseline;
using Miru.Consolables;
using Miru.Foundation.Bootstrap;
using Oakton;
using Oakton.Help;

namespace Miru.Foundation.Hosting
{
    public class CliMiruHost : IMiruHost
    {
        private readonly IMiruApp _app;
        private readonly MiruCommandCreator _commandCreator;
        private readonly ArgsConfiguration _argsConfig;

        public CliMiruHost(IMiruApp app, MiruCommandCreator commandCreator, ArgsConfiguration argsConfig)
        {
            _app = app;
            _commandCreator = commandCreator;
            _argsConfig = argsConfig;
        }

        public async Task RunAsync(CancellationToken token = default)
        {
            await RunAsync(string.Empty);
        }

        public async Task RunAsync(string args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            var factory = GetCommandFactory();

            var executor = new CommandExecutor(factory);
            
            if (args.IsNotEmpty())
                await executor.ExecuteAsync(args);
            else
                await executor.ExecuteAsync(_argsConfig.CliArgs);
        }

        private MiruCommandFactory GetCommandFactory()
        {
            var factory = new MiruCommandFactory(_commandCreator);

            RegisterAllTasks(factory);

            return factory;
        }

        private void RegisterAllTasks(MiruCommandFactory factory)
        {
            var cliCommands = _app.GetRegisteredServices<IConsolable>();
            
            foreach (var cliCommand in cliCommands)
            {
                factory.RegisterCommand(cliCommand);
            }

            factory.RegisterCommand<HelpCommand>();
        }
    }
}