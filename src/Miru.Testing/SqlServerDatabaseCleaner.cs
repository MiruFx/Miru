using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Respawn;
using Respawn.Graph;

namespace Miru.Testing;

public class SqlServerDatabaseCleaner : IDatabaseCleaner
{
    private readonly DatabaseOptions _dbOptions;

    public SqlServerDatabaseCleaner(IOptions<DatabaseOptions> appSettings)
    {
        _dbOptions = appSettings.Value;
    }

    public async Task ClearAsync()
    {
        var cleaner = await Respawner.CreateAsync(_dbOptions.ConnectionString, new RespawnerOptions()
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
        });

        await cleaner.ResetAsync(_dbOptions.ConnectionString);
    }
}