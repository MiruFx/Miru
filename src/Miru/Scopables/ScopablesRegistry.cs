using Microsoft.Extensions.DependencyInjection;

namespace Miru.Scopables;

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
}