using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Databases;

namespace Miru.Sqlite
{
    public class SqliteDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DbContext _dbContext;
        
        public SqliteDatabaseCleaner(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ClearAsync()
        {
            var sqlGetTables = @"
pragma foreign_keys = OFF;

select name 
from sqlite_master
where 
    type='table' and 
    name not in ('VersionInfo');";

            var sqlDelete = @"
delete from {0};
delete from SQLITE_SEQUENCE WHERE name='{0}';";

            using (var connection = _dbContext.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                await using (var tx = await connection.BeginTransactionAsync())
                {
                    var tables = await GetTables(connection, tx, sqlGetTables);

                    foreach (var table in tables)
                    {
                        await ExecuteSqlAsync(connection, tx, string.Format(sqlDelete, table));
                    }

                    await ExecuteSqlAsync(connection, tx, "pragma foreign_keys = ON;");
                
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