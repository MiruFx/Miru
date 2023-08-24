using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Storages;

namespace Miru;

public static class StorageRegistry
{
    public static IServiceCollection AddStorages(this IServiceCollection services)
    {
        return services
            .AddAppStorage<DefaultAppStorage>()
            .AddAssetsStorage<AssetsStorage>()
            .AddSingleton<ITempStorage, DefaultTempStorage>();
    }
    
    public static IServiceCollection AddAssetsStorage<TStorage>(this IServiceCollection services)
        where TStorage : class, IAssetsStorage
    {
        return services
            .AddSingleton<TStorage>()
            .AddSingleton<IAssetsStorage>(sp => sp.GetRequiredService<TStorage>());
    }
        
    public static IServiceCollection AddAppStorage<TStorage>(this IServiceCollection services)
        where TStorage : class, IAppStorage
    {
        return services
            .AddSingleton<TStorage>()
            .AddSingleton<IAppStorage>(sp => sp.GetRequiredService<TStorage>());
    }
    
    public static IServiceCollection AddStorage<TStorage>(this IServiceCollection services)
        where TStorage : class, IStorage
    {
        return services
            .AddSingleton<TStorage>()
            .AddSingleton<IStorage, TStorage>();
    }
}