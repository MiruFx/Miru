using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;
using Miru.Foundation.Bootstrap;
using Miru.Foundation.Hosting;
using Miru.Settings;

namespace Miru.Databases.EntityFramework
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

            services.AddSingleton(sp => new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(sp.GetService<DatabaseOptions>().ConnectionString)
                    .ScanIn(typeof(TDbContext).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false)
                .GetService<IMigrationRunner>());

            services.AddTransient<IDatabaseMigrator, FluentDatabaseMigrator>();
            
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