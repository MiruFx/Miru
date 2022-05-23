using Microsoft.Extensions.DependencyInjection;
using Miru.Scopables;

namespace Miru;

public static class ScopablesRegistry
{
    public static IServiceCollection AddCurrentAttributes<TCurrent, TCurrentAttributes>(
        this IServiceCollection services) 
        where TCurrent : class
        where TCurrentAttributes : class, ICurrentAttributes
    {
        return services
            .ReplaceScoped<TCurrent>()
            .ReplaceScoped<ICurrentAttributes, TCurrentAttributes>();
    }
    
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
        return services.Scan(scan => scan
            .FromAssemblies(typeof(TAssemblyOf).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IScopableQuery)))
            .AddClasses(classes => classes.AssignableTo(typeof(IScopableSaving)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}