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

        private static readonly Checkpoint Checkpoint = new()
        {
            DbAdapter = DbAdapter.MySql,
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
            await using var conn = new MySqlConnection(_dbOptions.ConnectionString);
            
            await conn.OpenAsync();

            await Checkpoint.Reset(conn);
        }
    }
}