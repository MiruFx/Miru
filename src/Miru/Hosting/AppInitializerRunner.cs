using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentMigrator.Runner;

namespace Miru.Hosting;

public class AppInitializerRunner : IAppInitializerRunner
{
    private readonly IEnumerable<IAppInitializer> _initializers;

    public AppInitializerRunner(IEnumerable<IAppInitializer> initializers)
    {
        _initializers = initializers;
    }

    public async Task RunAsync()
    {
        foreach (var appInitializer in _initializers)
        {
            var thread = new Thread(() => appInitializer.InitializeAsync().GetAwaiter().GetResult())
            {
                IsBackground = true
            };
            
            thread.Start();
        }

        await Task.CompletedTask;
    }
}