using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Baseline;
using Miru.Core;
using MiruCli.Process;

namespace MiruCli
{
    public class MiruCliOptions
    {
        public bool Verbose { get; set; }
        public string Project { get; set; }
    }
    
    public class RunOptions
    {
        public string Executable { get; set; }
        public string[] Args { get; set; }
    }
    
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Program().RunAsync(args);
        }

        private void Handle(MiruCliOptions options)
        {
            Console2.WhiteLine($"Verbose: {options.Verbose}");
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
                
                new WatchCommand("watch", m => m.AppDir)
            };
 
            rootCommand.Handler = CommandHandler.Create(
                (MiruCliOptions options, RunOptions runOptions) => 
                    RunAppMiru(options, runOptions));
                
            await rootCommand.InvokeAsync(args);
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
            
            var processRunner = new MiruProcessRunner(options.Verbose);

            var proc = processRunner.RunAsync(new ProcessSpec()
            {
                Executable = runOptions.Executable,
                Arguments = runOptions.Args,
                WorkingDirectory = directory(solution)
            });
        
            Task.WaitAll(proc);
        }
        
        public void RunAppMiru(
            MiruCliOptions options,
            RunOptions runOptions)
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
            
            if (runOptions.Args?.Length > 0)
                mergedArgs.AddRange(runOptions.Args);
        
            var proc = processRunner.RunAsync(new ProcessSpec
            {
                Executable = "dotnet",
                Arguments = mergedArgs,
                WorkingDirectory = solution.AppDir
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
