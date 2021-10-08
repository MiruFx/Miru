using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;

namespace Miru.Storages
{
    public static class StorageRegistry
    {
        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            return services.AddStorage<LocalDiskStorage>();
        }
        
        public static IServiceCollection AddStorage<TStorage>(this IServiceCollection services)
            where TStorage : class, IStorage
        {
            services.AddConsolable<StorageLinkConsolable>();
            
            services.AddSingleton<TStorage>();
            
            return services.AddSingleton<IStorage>(sp => sp.GetRequiredService<TStorage>());
        }
    }
}