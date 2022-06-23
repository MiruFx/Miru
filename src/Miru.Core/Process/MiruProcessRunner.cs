using System;
using System.Threading;
using System.Threading.Tasks;

namespace Miru.Core.Process;

public class MiruProcessRunner : IDisposable
{
    private readonly IConsole _console;
    private readonly CancellationTokenSource _cts;
    private readonly IReporter _reporter;

    public MiruProcessRunner(bool verbose = true, string prefix = "miru: ")
    {
        _console = PhysicalConsole.Singleton;
        _cts = new CancellationTokenSource();
        _reporter = new PrefixConsoleReporter(prefix, _console, verbose, false);
            
        _console.CancelKeyPress += OnCancelKeyPress;
    }

    public Task<int> RunAsync(ProcessSpec processSpec)
    {
        var processRunner = new ProcessRunner(_reporter);
            
        return processRunner.RunAsync(processSpec, _cts.Token);
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
        
    public void Dispose()
    {
        _console.CancelKeyPress -= OnCancelKeyPress;
        _cts.Dispose();
    }
}