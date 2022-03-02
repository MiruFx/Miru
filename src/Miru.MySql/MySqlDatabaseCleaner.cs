using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using MySqlConnector;
using Respawn;
using Respawn.Graph;

namespace Miru.MySql;

public class MySqlDatabaseCleaner : IDatabaseCleaner
{
    private readonly DatabaseCleanerOptions _databaseCleanerOptions;
    private readonly DatabaseOptions _dbOptions;

    public MySqlDatabaseCleaner(
        IOptions<DatabaseOptions> appSettings,
        IOptions<DatabaseCleanerOptions> databaseCleanerOptions)
    {
        _databaseCleanerOptions = databaseCleanerOptions.Value;
        _dbOptions = appSettings.Value;
    }

    public async Task ClearAsync()
    {
        await using var conn = new MySqlConnection(_dbOptions.ConnectionString);
            
        await conn.OpenAsync();

        var checkpoint = new Checkpoint
        {
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = _databaseCleanerOptions
                .TablesToIgnore
                .Select(x => new Table(x))
                .ToArray(),
            SchemasToInclude = new[] { conn.Database }
        };
            
        await checkpoint.Reset(conn);
    }
}