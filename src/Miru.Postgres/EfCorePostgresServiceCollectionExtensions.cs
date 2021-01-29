using System.Text;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Foundation.Hosting;
using Miru.Settings;

namespace Miru.Postgres
{
    public static class EfCorePostgresServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCorePostgres<TDbContext>(
            this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddEfCoreServices<TDbContext>();
            
            services.AddDbContext<TDbContext>((sp, options) =>
            {
                var dbOptions = sp.GetService<DatabaseOptions>();

                options.UseNpgsql(dbOptions.ConnectionString);

                if (sp.GetService<IHostEnvironment>().IsDevelopmentOrTest())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddMigrator<TDbContext>(mb => mb.AddPostgres92());
            
            return services;
        }
    }
}