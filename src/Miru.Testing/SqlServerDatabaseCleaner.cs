using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using Respawn;

namespace Miru.Testing
{
    public class SqlServerDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DatabaseOptions _dbOptions;

        private static readonly Checkpoint Checkpoint = new Checkpoint
        {
            TablesToIgnore = new[]
            {
                "sysdiagrams",
                "tblUser",
                "tblObjectType",
                "__MigrationHistory",
                "VersionInfo"
            },
            SchemasToExclude = new string[] { }
        };

        public SqlServerDatabaseCleaner(IOptions<DatabaseOptions> appSettings)
        {
            _dbOptions = appSettings.Value;
        }

        public async Task Clear()
        {
            await Checkpoint.Reset(_dbOptions.ConnectionString);
        }
    }
}