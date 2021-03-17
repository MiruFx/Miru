using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using MySqlConnector;
using Respawn;

namespace Miru.MySql
{
    public class MySqlDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DatabaseOptions _dbOptions;

        public MySqlDatabaseCleaner(IOptions<DatabaseOptions> appSettings)
        {
            _dbOptions = appSettings.Value;
        }

        public async Task ClearAsync()
        {
            await using var conn = new MySqlConnection(_dbOptions.ConnectionString);
            
            await conn.OpenAsync();

            var checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.MySql,
                TablesToIgnore = new[]
                {
                    "__MigrationHistory",
                    "VersionInfo",
                    "__efmigrationshistory"
                },
                SchemasToInclude = new[] { conn.Database }
            };
            
            await checkpoint.Reset(conn);
        }
    }
}