using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Baseline;
using Miru.Cli.Process;
using Miru.Core;

namespace Miru.Cli;

public class Program
{
    public static async Task Main(string[] args)
    {
        await new Program().RunAsync(args);
    }

    private async Task RunAsync(string[] args)
    {
        var rootCommand = new RootCommand
        {
            new Option<bool>(new[] { "--verbose"}),
            new Option<string>(new[] { "--project", "-p"}),
                
            new NewCommand("new"),
                
            new RunAtCommand("app") 
            {
                Handler = CommandHandler.Create((MiruCliOptions options, RunOptions runOptions) => 
                    RunAtAsync(options, runOptions, s => s.AppDir))
            },
                
            new RunAtCommand("test") 
            {
                Handler = CommandHandler.Create((MiruCliOptions options, RunOptions runOptions) => 
                    RunAtAsync(options, runOptions, s => s.AppTestsDir))
            },
                
            new RunAtCommand("pagetest")
            {
                Handler = CommandHandler.Create((MiruCliOptions options, RunOptions runOptions) => 
                    RunAtAsync(options, runOptions, s => s.AppPageTestsDir))
            },
                
            new WatchCommand("watch", m => m.AppDir)
        };

        var result = rootCommand.Parse(args);

        if (result.CommandResult.Command.Name.Equals("Miru.Cli"))
            await RunMiruAsync(new MiruCliOptions(), new RunMiruOptions { MiruArgs = args });
        else if (result.CommandResult.Command is not RootCommand)
            await result.InvokeAsync();
        else
            await rootCommand.InvokeAsync(args);
    }

    public async Task RunAtAsync(
        MiruCliOptions options,
        RunOptions runOptions,
        Func<MiruSolution, MiruPath> directory)
    {
        var result = FindSolution(options);
            
        if (result.FoundSolution == false)
        {
            Console2.RedLine($"There is no Miru's Solution at {result.LookedAt}");
            return;
        }

        var solution = result.Solution;

        var exec = OS.IsWindows ? 
            OS.WhereOrWhich(runOptions.Executable) :
            runOptions.Executable;

        var processRunner = new MiruProcessRunner(options.Verbose, string.Empty);

        await processRunner.RunAsync(new ProcessSpec()
        {
            Executable = exec,
            Arguments = runOptions.Args,
            WorkingDirectory = directory(solution)
        });
    }
        
    public async Task RunMiruAsync(
        MiruCliOptions options,
        RunMiruOptions runMiruOptions)
    {
        var result = FindSolution(options);
            
        if (result.FoundSolution == false)
        {
            Console2.RedLine($"There is no Miru's Solution at {result.LookedAt}");
            return;
        }
            
        var solution = result.Solution;
            
        var processRunner = new MiruProcessRunner(options.Verbose);
            
        var mergedArgs = new List<string>();
            
        mergedArgs.AddRange(new[] {"run", "--no-build", "--", "miru"});
            
        if (runMiruOptions.MiruArgs?.Length > 0)
            mergedArgs.AddRange(runMiruOptions.MiruArgs);
        
        // TODO: handle exception: error when running 'command'
        await processRunner.RunAsync(new ProcessSpec
        {
            Executable = "dotnet",
            Arguments = mergedArgs,
            WorkingDirectory = solution.AppDir
        });
    }
        
    private SolutionFinderResult FindSolution(MiruCliOptions options)
    {
        var solutionDir = options.Project.IsNotEmpty() ? options.Project : Directory.GetCurrentDirectory();
        
        return new SolutionFinder().FromDir(solutionDir);
    }
}