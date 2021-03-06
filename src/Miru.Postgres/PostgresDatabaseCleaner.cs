﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Settings;
using Respawn;

namespace Miru.Postgres
{
    public class PostgresDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DatabaseCleanerOptions _databaseCleanerOptions;
        private readonly DatabaseOptions _databaseOptions;

        public PostgresDatabaseCleaner(
            IOptions<DatabaseOptions> databaseOptions,
            IOptions<DatabaseCleanerOptions> databaseCleanerOptions)
        {
            _databaseCleanerOptions = databaseCleanerOptions.Value;
            _databaseOptions = databaseOptions.Value;
        }

        public async Task ClearAsync()
        {
            var checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = _databaseCleanerOptions.TablesToIgnore.ToArray(),
                SchemasToExclude = new string[] { }
            };
            
            await checkpoint.Reset(_databaseOptions.ConnectionString);
        }
    }
}