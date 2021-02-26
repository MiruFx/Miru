using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using Respawn;

namespace Miru.MySql
{
    public class MySqlDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DatabaseOptions _dbOptions;

        private static readonly Checkpoint Checkpoint = new()
        {
            TablesToIgnore = new[]
            {
                "__MigrationHistory",
                "VersionInfo",
                "__efmigrationshistory"
            },
            SchemasToExclude = new string[] { }
        };

        public MySqlDatabaseCleaner(IOptions<DatabaseOptions> appSettings)
        {
            _dbOptions = appSettings.Value;
        }

        public async Task ClearAsync()
        {
            await Checkpoint.Reset(_dbOptions.ConnectionString);
        }
    }
}