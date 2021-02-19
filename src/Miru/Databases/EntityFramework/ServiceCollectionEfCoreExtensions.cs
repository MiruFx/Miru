using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Databases.EntityFramework
{
    public static class ServiceCollectionEfCoreExtensions
    {
        public static IServiceCollection AddEfCoreServices<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IDataAccess, EntityFrameworkDataAccess>();
            
            services.AddTransient<IDatabaseCreator, EntityFrameworkDatabaseCreator>();
            
            // Forward
            services.ForwardScoped<DbContext, TDbContext>();

            return services;
        }
    }
}