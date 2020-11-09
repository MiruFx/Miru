using Microsoft.Extensions.DependencyInjection;

namespace Miru.Databases.EntityFramework
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddBeforeSaveHandler<TBeforeSaveHandler>(this IServiceCollection services) 
            where TBeforeSaveHandler : class, IBeforeSaveHandler
        {
            return services.AddScoped<IBeforeSaveHandler, TBeforeSaveHandler>();
        }
    }
}