using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Settings;

namespace Miru.Databases.EntityFramework
{
    public static class ServiceCollectionEfCoreExtensions
    {
        public static IServiceCollection AddEfCoreServices<TDbContext>(this IServiceCollection services) 
            where TDbContext : DbContext
        {
            services.AddScoped<IDataAccess, EntityFrameworkDataAccess>();
            
            services.AddTransient<IDatabaseCreator, EntityFrameworkDatabaseCreator>();
            
            // Forward
            services.ForwardScoped<DbContext, TDbContext>();

            // ConnectionString transformation
            services.PostConfigureAll<DatabaseOptions>(settings =>
            {
                var dbDir = App.Solution.StorageDir / "db" / ".";
                
                settings.ConnectionString = 
                    settings.ConnectionString?.Replace("{{ db_dir }}", dbDir);
                
                Directories.CreateIfNotExists(dbDir);
            });
            
            return services;
        }
    }
}