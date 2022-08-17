using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentMigrator.Runner;

namespace Miru.Hosting;

public class HostInitializedRunner : IHostInitializedRunner
{
    private readonly IEnumerable<IHostInitialized> _initializedListeners;

    public HostInitializedRunner(IEnumerable<IHostInitialized> initializers)
    {
        _initializedListeners = initializers;
    }

    public async Task RunAsync()
    {
        foreach (var initialized in _initializedListeners)
        {
            await initialized.InitializeAsync();
        }
    }
}