using Microsoft.Extensions.Logging;

namespace Miru.Databases.Migrations
{
    public class NoDatabaseMigrator : IDatabaseMigrator
    {
        private readonly ILogger<IDatabaseMigrator> _logger;

        public NoDatabaseMigrator(ILogger<IDatabaseMigrator> logger)
        {
            _logger = logger;
        }

        public void UpdateSchema()
        {
            _logger.LogInformation("No database migrator configured");
        }

        public void DowngradeSchema(int steps = 1)
        {
            _logger.LogInformation("No database migrator configured");
        }
        
        public void RecreateSchema()
        {
            _logger.LogInformation("No database migrator configured");
        }
    }
}
