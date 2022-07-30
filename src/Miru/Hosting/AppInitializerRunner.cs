using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miru.Hosting;

public class AppInitializerRunner
{
    private readonly IEnumerable<IAppInitializer> _initializers;

    public AppInitializerRunner(IEnumerable<IAppInitializer> initializers)
    {
        _initializers = initializers;
    }

    public async Task RunAsync()
    {
        await Parallel.ForEachAsync(
            _initializers, 
            new ParallelOptions { MaxDegreeOfParallelism = 1 }, 
            async (initializer, _) =>
            {
                await initializer.InitializeAsync();
            });
    }
}