using Microsoft.Extensions.DependencyInjection;

namespace Miru.Scopables;

public static class ScopablesRegistry
{
    public static IServiceCollection AddCurrentScope<TCurrent, TCurrentScope>(
        this IServiceCollection services) 
        where TCurrent : class
        where TCurrentScope : class, ICurrentScope
    {
        return services
            .AddScoped<TCurrent>()
            .AddScoped<ICurrentScope, TCurrentScope>();
    }
    
    public static IServiceCollection AddScopable<TScope>(
        this IServiceCollection services) where TScope : Scopable
    {
        return services
            .AddScoped<IScopableQuery, TScope>()
            .AddScoped<IScopableSaving, TScope>();
    }
}