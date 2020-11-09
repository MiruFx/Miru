using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Exceptions;
using Microsoft.Extensions.Logging;

namespace Miru.Databases.Migrations.FluentMigrator
{
    public class FluentDatabaseMigrator : IDatabaseMigrator
    {
        private readonly IMigrationRunner _migrationRunner;
        private readonly ILogger<FluentDatabaseMigrator> _logger;

        public FluentDatabaseMigrator(
            IMigrationRunner migrationRunner, 
            ILogger<FluentDatabaseMigrator> logger)
        {
            _migrationRunner = migrationRunner;
            _logger = logger;
        }

        public void UpdateSchema()
        {
            _logger.LogInformation("Updating database schema");
            
            Execute(m => m.MigrateUp());
            
            _logger.LogInformation("Database schema updated");
        }

        public void DowngradeSchema(int steps = 1)
        {
            _logger.LogInformation("Downgrading database schema");
            
            Execute(m => m.Rollback(steps));
            
            _logger.LogInformation("Database downgraded");
        }

        public void RecreateSchema()
        {
            _logger.LogInformation("Recreating database schema");
            
            _logger.LogInformation("Downgrading");
            
            Execute(m => m.MigrateDown(0));
            
            _logger.LogInformation("Upgrading");
            
            Execute(m => m.MigrateUp());
            
            _logger.LogInformation("Database schema recreated");
        }

        private void Execute(Action<IMigrationRunner> action)
        {
            try
            {
                action(_migrationRunner);
            }
            catch (MissingMigrationsException)
            {
                _logger.LogInformation("No migrations found");
            }
        }
    }
}