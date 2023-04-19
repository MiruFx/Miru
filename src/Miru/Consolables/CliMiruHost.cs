using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using Miru.Foundation.Bootstrap;
using Miru.Hosting;

namespace Miru.Consolables;

public class CliMiruHost : ICliMiruHost
{
    private readonly IEnumerable<Consolable> _consolables;
    private readonly ArgsConfiguration _argsConfig;
    private readonly IMiruApp _app;

    public CliMiruHost(IEnumerable<Consolable> commands, ArgsConfiguration argsConfig, IMiruApp app)
    {
        _consolables = commands;
        _argsConfig = argsConfig;
        _app = app;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        await RunAsync(_argsConfig.CliArgs.Join(" "));
    }

    public async Task RunAsync(string args)
    {
        var rootCommand = CreateRootCommand();

        AddConsolables(rootCommand);

        var result = rootCommand.Parse(args);
            
        if (result.Errors.None() &&
            result.CommandResult.Command is not RootCommand && 
            result.CommandResult.Command is { } command)
        {
            // TODO: run ICliHostStartup
            await InvokeConsolableAsync(command, result);
        }
        else
        {
            await rootCommand.InvokeAsync(args);
        }
    }

    private async Task InvokeConsolableAsync(Command command, ParseResult result)
    {
        using var scope = _app.Get<IServiceProvider>().CreateScope();

        var handlerType = command.GetType().Assembly.GetType($"{command.GetType().FullName}+ConsolableHandler");
        var handler = (IConsolableHandler)scope.ServiceProvider.GetRequiredService(handlerType!);

        command.Handler = CommandHandler.Create(
            handlerType.GetMethod(nameof(IConsolableHandler.Execute))!,
            handler);

        await result.InvokeAsync();
    }

    private void AddConsolables(RootCommand rootCommand)
    {
        foreach (var consolable in _consolables)
        {
            rootCommand.AddCommand(consolable);
        }
    }

    public static RootCommand CreateRootCommand()
    {
        var rootCommand = new RootCommand
        {
            Name = "miru",

            // unknown options or arguments passed to MiruCli or Consolables
            // are not treated as errors 
            TreatUnmatchedTokensAsErrors = false
        };

        var environmentOption = new Option<string>(
                "--environment", 
                "Executes command in the specified environment")
            .WithAlias("--e");
        
        rootCommand.AddGlobalOption(environmentOption);
        
        return rootCommand;
    }
}