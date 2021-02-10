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
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // var result = new SolutionFinder().FromDir(@"D:\Intan\Temp\Intanext.Shopworld\src\Intanext.Shopworld");
            var result = new SolutionFinder().FromCurrentDir();

            Console2.WhiteLine(MiruPath.CurrentPath);
            Console2.WhiteLine(AppContext.BaseDirectory);
            Console2.WhiteLine(Directory.GetCurrentDirectory());
            
            if (result.FoundSolution == false)
            {
                var rootCommand = new RootCommand
                {
                    // new ShowProjectCommand("app", m => m.AppDir),
                    // new ShowProjectCommand("tests", m => m.AppTestsDir),
                    // new ShowProjectCommand("pagetests", m => m.AppPageTestsDir),
                    // new SetupCommand("setup"),
                    
                    new NewCommand("new"),
                };

                rootCommand.Name = "miru";
            
                await rootCommand.InvokeAsync(args);
            }
            else
            {
                var solution = result.Solution;
                
                var rootCommand = new RootCommand
                {
                    new AtAppCommand("app", solution),
                    new AtTestCommand("test", solution),
                    new AtPageTestCommand("pagetest", solution),
                    new WatchCommand("watch", m => m.AppDir),
                };
                
                rootCommand.AddArgument(new Argument<string[]>("args") { Arity = ArgumentArity.ZeroOrMore });
                rootCommand.Handler = CommandHandler.Create((string[] miruArgs) => Execute(solution, miruArgs));
                
                await rootCommand.InvokeAsync(args);
            }
        }

        public static void Execute(MiruSolution solution, string[] args)
        {
            var processRunner = new MiruProcessRunner(prefix: string.Empty);
            var mergedArgs = new List<string>();
            mergedArgs.AddRange(new[] {"run", "--no-build", "--", "miru"});
            
            if (args?.Length > 0)
                mergedArgs.AddRange(args);

            var proc = processRunner.RunAsync(new ProcessSpec()
            {
                Executable = "dotnet",
                Arguments = mergedArgs,
                WorkingDirectory = solution.AppDir
            });

            Task.WaitAll(proc);
        }
    }
}
