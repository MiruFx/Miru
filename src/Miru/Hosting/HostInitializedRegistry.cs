using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Miru.Hosting;

public static class HostInitializedRegistry
{
    public static IServiceCollection AddHostInitialized<THostInitialized>(this IServiceCollection services)
        where THostInitialized : class, IHostInitialized
    {
        return services.AddHostInitialized(typeof(THostInitialized));
    }
    
    public static IServiceCollection AddHostInitialized(this IServiceCollection services, Type initializerType)
    {
        services.TryAddTransient<HostInitializedRunner>();

        return services.AddTransient(typeof(IHostInitialized), initializerType);
    }
    
    public static IServiceCollection AddHostInitializedOf<TAssemblyOfType>(this IServiceCollection services)
    {
        var initializerTypes = typeof(TAssemblyOfType).Assembly
            .ExportedTypes
            .Where(x => x.Implements<IHostInitialized>() && x.IsAbstract == false);

        foreach (var initializerType in initializerTypes)
        {
            services.AddHostInitialized(initializerType);
        }
            
        return services;
    }
}