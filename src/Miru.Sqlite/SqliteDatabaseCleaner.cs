using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Miru.Databases;
using Miru.Settings;

namespace Miru.Sqlite
{
    public class SqliteDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DatabaseOptions _databaseOptions;
        
        public SqliteDatabaseCleaner(DatabaseOptions databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public async Task ClearAsync()
        {
            // FIXME: Configurable tables names
            var sqlGetTables = @"
select name 
from sqlite_master
where 
    type='table' and 
    name not in ('VersionInfo', '__MigrationHistory', '__efmigrationshistory');";

            var sqlDelete = @"
PRAGMA foreign_keys = OFF;
delete from {0};
delete from SQLITE_SEQUENCE WHERE name='{0}';
PRAGMA foreign_keys = ON;";

            using (var connection = new SqliteConnection($"{_databaseOptions.ConnectionString};foreign keys=false;"))
            {
                await connection.OpenAsync();

                await using (var tx = await connection.BeginTransactionAsync())
                {
                    var tables = await GetTables(connection, tx, sqlGetTables);

                    foreach (var table in tables)
                    {
                        await ExecuteSqlAsync(connection, tx, string.Format(sqlDelete, table));
                    }

                    await tx.CommitAsync();
                }
                
                await connection.CloseAsync();
            }
        }
        
        private async Task<List<string>> GetTables(DbConnection connection, DbTransaction tx, string sql)
        {
            var tables = new List<string>();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandTimeout = cmd.CommandTimeout;
                cmd.CommandText = sql;
                cmd.Transaction = tx;
                
                var reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader[0].ToString());
                    }
                }
            }

            return tables;
        }
        
        private async Task ExecuteSqlAsync(DbConnection connection, DbTransaction tx, string sql)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Transaction = tx;

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}