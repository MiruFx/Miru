using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Scopables;

namespace Miru;

public static class ScopablesRegistry
{
    public static IServiceCollection AddScopable<TScope>(
        this IServiceCollection services) where TScope : Scopable
    {
        return services
            .AddScoped<IScopableQuery, TScope>()
            .AddScoped<IScopableSaving, TScope>();
    }
    
    public static IServiceCollection AddScopables<TAssemblyOf>(
        this IServiceCollection services)
    {
        services.AddTransient<IInterceptor, ScopableInterceptor>();
            
        return services.Scan(scan => scan
            .FromAssemblies(typeof(TAssemblyOf).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IScopableQuery)))
            .AddClasses(classes => classes.AssignableTo(typeof(IScopableSaving)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
    
    
    // public static IServiceCollection AddScopables2<TAssemblyOf>(
    //     this IServiceCollection services)
    // {
    //     services.AddTransient<IInterceptor, ScopableInterceptor2>();
    //         
    //     return services.Scan(scan => scan
    //         .FromAssemblies(typeof(TAssemblyOf).Assembly)
    //         .AddClasses(classes => classes.AssignableTo(typeof(IScopableQuery)))
    //         .AddClasses(classes => classes.AssignableTo(typeof(IScopableSaving)))
    //         .AsImplementedInterfaces()
    //         .WithScopedLifetime());
    // }

}