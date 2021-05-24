using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Storages;

namespace Miru.Testing
{
    public static class StorageTestServiceCollectionExtensions
    {
        public static IServiceCollection AddTestStorage(this IServiceCollection services)
        {
            services.AddConsolable<StorageLinkConsolable>();
            
            return services.AddStorage<TestStorage>();
        }
    }
}