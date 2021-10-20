using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Baseline;
using Miru.Cli.Process;
using Miru.Core;

namespace Miru.Cli
{
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
                        RunAt(options, runOptions, s => s.AppDir))
                },
                
                new RunAtCommand("test") 
                {
                    Handler = CommandHandler.Create((MiruCliOptions options, RunOptions runOptions) => 
                        RunAt(options, runOptions, s => s.AppTestsDir))
                },
                
                new RunAtCommand("pagetest")
                {
                    Handler = CommandHandler.Create((MiruCliOptions options, RunOptions runOptions) => 
                        RunAt(options, runOptions, s => s.AppPageTestsDir))
                },
                
                new WatchCommand("watch", m => m.AppDir),
                
                new Argument<string[]>("miru-args")
                {
                    Arity = ArgumentArity.ZeroOrMore
                },
            };

            rootCommand.Name = "miru";
            
            rootCommand.Handler = CommandHandler.Create(
                (MiruCliOptions options, RunMiruOptions runOptions) => 
                    RunMiru(options, runOptions));

            var result = rootCommand.Parse(args);

            // if command is miru, should just pass the arguments to the app
            if (result.CommandResult.Command.Name.Equals("miru"))
            {
                RunMiru(new MiruCliOptions(), new RunMiruOptions { MiruArgs = args });
                await Task.CompletedTask;
            }
            else
            {
                await rootCommand.InvokeAsync(args);
            }
        }

        public void RunAt(
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

            var proc = processRunner.RunAsync(new ProcessSpec()
            {
                Executable = exec,
                Arguments = runOptions.Args,
                WorkingDirectory = directory(solution)
            });
        
            Task.WaitAll(proc);
        }
        
        public void RunMiru(
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
            var proc = processRunner.RunAsync(new ProcessSpec
            {
                Executable = "dotnet",
                Arguments = mergedArgs,
                WorkingDirectory = solution.AppDir
                // WorkingDirectory = @"D:\Projects\Miru\samples\Playground\src\Playground"
            });
        
            Task.WaitAll(proc);
        }
        
        private SolutionFinderResult FindSolution(MiruCliOptions options)
        {
            var solutionDir = options.Project.IsNotEmpty() ? options.Project : Directory.GetCurrentDirectory();
        
            return new SolutionFinder().FromDir(solutionDir);
        }
    }
}
