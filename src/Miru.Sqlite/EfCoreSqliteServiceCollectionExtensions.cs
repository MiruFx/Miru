using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Foundation.Hosting;
using Miru.Settings;

namespace Miru.Sqlite
{
    public static class EfCoreSqliteServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCoreSqlite<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddEfCoreServices<TDbContext>();
            
            services.AddDbContext<TDbContext>((sp, options) =>
            {
                var dbConfig = sp.GetService<DatabaseOptions>();
                
                options.UseSqlite(dbConfig.ConnectionString);

                if (sp.GetService<IHostEnvironment>().IsDevelopmentOrTest())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddMigrator<TDbContext>(mb => mb.AddSQLite());
            
            return services;
        }
    }
}