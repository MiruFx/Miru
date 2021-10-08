using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Miru.Foundation.Hosting
{
    public static class HostBuilderExtensions
    {
        public static async Task RunMiruAsync(this IHostBuilder hostBuilder)
        {
            var host = hostBuilder.Build();

            var miruRunner = host.Services.GetRequiredService<MiruRunner>();

            await miruRunner.RunAsync();
        }

        public static IMiruApp BuildApp(this IHostBuilder hostBuilder)
        {
            return hostBuilder.Build().Services.GetService<IMiruApp>();
        }
    }
}