using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Exceptions;

namespace Miru.Databases.Migrations.FluentMigrator;

public class FluentDatabaseMigrator : IDatabaseMigrator
{
    private readonly IMigrationRunner _migrationRunner;

    public FluentDatabaseMigrator(IMigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    public void UpdateSchema()
    {
        App.Framework.Information("Updating database schema");
            
        Execute(m => m.MigrateUp());
            
        App.Framework.Information("Database schema updated");
    }

    public void DowngradeSchema(int steps = 1)
    {
        App.Framework.Information("Downgrading database schema");
            
        Execute(m => m.Rollback(steps));
            
        App.Framework.Information("Database downgraded");
    }

    public void RecreateSchema()
    {
        App.Framework.Information("Recreating database schema");
            
        App.Framework.Information("Downgrading");
            
        Execute(m => m.MigrateDown(0));
            
        App.Framework.Information("Upgrading");
            
        Execute(m => m.MigrateUp());
            
        App.Framework.Information("Database schema recreated");
    }

    private void Execute(Action<IMigrationRunner> action)
    {
        try
        {
            action(_migrationRunner);
        }
        catch (MissingMigrationsException)
        {
            App.Framework.Information("No Migrations were found in your application");
        }
    }
}