namespace Miru.Currentable;

public static class CurrentableRegistry
{
    public static IServiceCollection AddCurrent<TCurrent, TCurrentHandler>(
        this IServiceCollection services) 
        where TCurrent : class
        where TCurrentHandler : class, ICurrentHandler
    {
        return services
            .ReplaceScoped<TCurrent>()
            .ReplaceScoped<ICurrentHandler, TCurrentHandler>();
    }
}