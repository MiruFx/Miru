using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Miru.Hosting;

public static class AppInitializerRegistry
{
    public static IServiceCollection AddInitializer<TAppInitializer>(this IServiceCollection services)
        where TAppInitializer : class, IAppInitializer
    {
        return services.AddInitializer(typeof(TAppInitializer));
    }
    
    public static IServiceCollection AddInitializer(this IServiceCollection services, Type initializerType)
    {
        services.TryAddSingleton<AppInitializerRunner>();

        return services.AddTransient(typeof(IAppInitializer), initializerType);
    }
    
    public static IServiceCollection AddInitializers<TAssemblyOfType>(this IServiceCollection services)
    {
        var initializerTypes = typeof(TAssemblyOfType).Assembly
            .ExportedTypes
            .Where(x => x.Implements<IAppInitializer>() && x.IsAbstract == false);

        foreach (var initializerType in initializerTypes)
        {
            services.AddInitializer(initializerType);
        }
            
        return services;
    }
}