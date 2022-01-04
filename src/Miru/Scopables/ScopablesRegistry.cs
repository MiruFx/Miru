using Microsoft.Extensions.DependencyInjection;
using Miru.Scopables;

namespace SanusPlus.Infra.Scopables;

public static class ScopablesRegistry
{
    public static IServiceCollection AddScopable<TScope>(
        this IServiceCollection services) where TScope : Scopable
    {
        return services
            .AddScoped<IScopableQuery, TScope>()
            .AddScoped<IScopableSaving, TScope>();
    }
}