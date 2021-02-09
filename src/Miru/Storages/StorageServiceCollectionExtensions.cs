using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Foundation;

namespace Miru.Storages
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            services.AddConsolable<StorageLinkConsolable>();
            
            return services.AddSingleton<Storage>();
        }
    }
}