using FluentMigrator.Runner;
using Miru.Databases;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;

namespace Miru;

public static class MigrationRegistry
{
    public static IServiceCollection AddMigrator<TStartup>(
        this IServiceCollection services,
        Action<IMigrationRunnerBuilder> migrationRunnerBuilder = null)
    {
        services.AddTransient<IDatabaseMigrator, FluentDatabaseMigrator>();
        services.AddOptions<MigrationOptions>();

        services.AddConsolable<DbResetConsolable>();
        services.AddConsolable<DbRollbackConsolable>();
        services.AddConsolable<DbMigrateConsolable>();
            
        services.AddTransient(sp => new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                migrationRunnerBuilder?.Invoke(rb);

                var migrationConnectionString =
                    sp.GetRequiredService<IOptions<MigrationOptions>>().Value?.ConnectionString;

                if (migrationConnectionString.IsEmpty())
                    migrationConnectionString =
                        sp.GetRequiredService<IOptions<DatabaseOptions>>().Value.ConnectionString;
                
                rb.WithGlobalConnectionString(migrationConnectionString)
                    .ScanIn(typeof(TStartup).Assembly)
                    .For
                    .All();
            })
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false)
            .GetService<IMigrationRunner>());
            
        // ConnectionString transformation
        services.PostConfigureAll<MigrationOptions>(settings =>
        {
            if (App.Solution != null)
            {
                var dbDir = App.Solution.StorageDir / "db" / ".";
                
                settings.ConnectionString = 
                    settings.ConnectionString?.Replace("{{ db_dir }}", dbDir);
                
                dbDir.EnsureDirExist();
            }
        });
        
        return services;
    }
}