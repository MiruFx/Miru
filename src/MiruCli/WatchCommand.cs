using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Baseline;
using ImTools;
using Miru.Core;
using Miru.Core.Makers;
using MiruCli.Process;
using IConsole = MiruCli.Process.IConsole;

namespace MiruCli
{
    public class WatchCommand : Command, IDisposable
    {
        private readonly IConsole _console;
        private readonly CancellationTokenSource _cts;
        private readonly IReporter _reporter;
        
        public WatchCommand(string commandName, Func<MiruSolution, string> func) : base(commandName)
        {
            Handler = CommandHandler.Create(() => WatchRun(func));
            
            _console = PhysicalConsole.Singleton;
            _cts = new CancellationTokenSource();
            _reporter = CreateReporter(verbose: true, quiet: false, console: _console);

            _console.CancelKeyPress += OnCancelKeyPress;
        }
        
        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            // suppress CTRL+C on the first press
            args.Cancel = !_cts.IsCancellationRequested;
        
            if (args.Cancel)
            {
                _reporter.Output("Shutdown requested. Press Ctrl+C again to force exit.");
            }
        
            _cts.Cancel();
        }
        
        private static bool IsGlobalVerbose()
        {
            bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_CLI_CONTEXT_VERBOSE"), out bool globalVerbose);
            return globalVerbose;
        }
        
        public void Dispose()
        {
            _console.CancelKeyPress -= OnCancelKeyPress;
            _cts.Dispose();
        }
        
        private static IReporter CreateReporter(bool verbose, bool quiet, IConsole console)
            => new PrefixConsoleReporter("miru-watch : ", console, verbose || IsGlobalVerbose(), quiet);
        
        private void WatchRun(Func<MiruSolution, string> func)
        {
            var solutionFinder = new SolutionFinder(new FileSystem());            
            var solution = solutionFinder.FromDir(Directory.GetCurrentDirectory());
            
            var processRunner = new ProcessRunner(_reporter);
            
            // FIXME: npm and dotnet path finder: 
            //  Win: where
            //  bash: which
            var webpack = new ProcessSpec()
            {
                Executable = OS.IsWindows ? "c:\\Program Files\\nodejs\\npm.cmd" : "npm",
                WorkingDirectory = solution.Solution.AppDir,
                Arguments = new[] { "--prefix", solution.Solution.AppDir.ToString(), "run", "watch" },
            };
            
            var dotnet = new ProcessSpec()
            {
                Executable = "dotnet",
                WorkingDirectory = solution.Solution.AppDir,
                Arguments = new[] { "watch", "run" }
            };
            
            var dotnetRunner = processRunner.RunAsync(dotnet, _cts.Token);
            var webpackRunner = processRunner.RunAsync(webpack, _cts.Token);
            
            Task.WaitAll(dotnetRunner);
            Task.WaitAll(dotnetRunner, webpackRunner);
        }
    }
}