using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using Respawn;

namespace Miru.Postgres
{
    public class PostgresDatabaseCleaner : IDatabaseCleaner
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

        public PostgresDatabaseCleaner(IOptions<DatabaseOptions> appSettings)
        {
            _dbOptions = appSettings.Value;
        }

        public async Task Clear()
        {
            await Checkpoint.Reset(_dbOptions.ConnectionString);
        }
    }
}