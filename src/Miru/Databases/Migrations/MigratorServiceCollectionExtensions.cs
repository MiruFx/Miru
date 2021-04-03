using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Core;
using Miru.Databases.Migrations.FluentMigrator;
using Miru.Settings;

namespace Miru.Databases.Migrations
{
    public static class MigratorServiceCollectionExtensions
    {
        public static IServiceCollection AddMigrator<TStartup>(
            this IServiceCollection services,
            Action<IMigrationRunnerBuilder> migrationRunnerBuilder = null)
        {
            services.AddTransient<IDatabaseMigrator, FluentDatabaseMigrator>();

            services.AddConsolable<DbResetConsolable>();
            services.AddConsolable<DbRollbackConsolable>();
            services.AddConsolable<DbMigrateConsolable>();
            
            services.AddTransient(sp => new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    migrationRunnerBuilder?.Invoke(rb);
                    
                    rb.WithGlobalConnectionString(sp.GetService<DatabaseOptions>().ConnectionString)
                        .ScanIn(typeof(TStartup).Assembly).For.All();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false)
                .GetService<IMigrationRunner>());
            
            return services;
        }
    }
}