using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using Respawn;
using Respawn.Graph;

namespace Miru.Testing
{
    public class SqlServerDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DatabaseOptions _dbOptions;

        private static readonly Checkpoint Checkpoint = new Checkpoint
        {
            TablesToIgnore = new[]
            {
                new Table("sysdiagrams"),
                new Table("tblUser"),
                new Table("tblObjectType"),
                new Table("__MigrationHistory"),
                new Table("VersionInfo")
            },
            SchemasToExclude = new string[] { }
        };

        public SqlServerDatabaseCleaner(IOptions<DatabaseOptions> appSettings)
        {
            _dbOptions = appSettings.Value;
        }

        public async Task ClearAsync()
        {
            await Checkpoint.Reset(_dbOptions.ConnectionString);
        }
    }
}