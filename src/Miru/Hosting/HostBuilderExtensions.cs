using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Miru.Hosting;

public static class HostBuilderExtensions
{
    public static async Task<IMiruHost> RunMiruAsync(this IHostBuilder hostBuilder)
    {
        var host = hostBuilder.Build();

        var miruHost = host.Services.GetRequiredService<IMiruHost>();
        
        await host.RunAppInitializers();

        await miruHost.RunAsync();

        return miruHost;
    }

    public static IMiruApp BuildApp(this IHostBuilder hostBuilder)
    {
        return hostBuilder.Build().Services.GetService<IMiruApp>();
    }
    
    public static async Task RunAppInitializers(this IHost host)
    {
        using var scope = host.Services.Get<IMiruApp>().WithScope();

        var appInitializerRunner = scope.Get<AppInitializerRunner>();

        if (appInitializerRunner is not null)
        {
            await appInitializerRunner.RunAsync();
        }
    }
}