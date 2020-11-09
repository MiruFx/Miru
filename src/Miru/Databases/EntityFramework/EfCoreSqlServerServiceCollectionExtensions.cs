using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;
using Miru.Foundation.Bootstrap;
using Miru.Foundation.Hosting;
using Miru.Settings;

namespace Miru.Databases.EntityFramework
{
    public static class EfCoreSqlServerServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCoreSqlServer<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddEfCoreServices<TDbContext>();

            services.AddDbContext<TDbContext>((sp, options) =>
            {
                var dbConfig = sp.GetService<DatabaseOptions>();
                
                options.UseSqlServer(dbConfig.ConnectionString);

                // TODO: add action to configure options from outside this method
                if (sp.GetService<IHostEnvironment>().IsDevelopmentOrTest())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            // migration
            services.AddSingleton(sp => new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer2016()
                    .WithGlobalConnectionString(sp.GetService<DatabaseOptions>().ConnectionString)
                    .ScanIn(typeof(TDbContext).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false)
                .GetService<IMigrationRunner>());
            
            services.AddTransient<IDatabaseMigrator, FluentDatabaseMigrator>();

            return services;
        }
    }
}