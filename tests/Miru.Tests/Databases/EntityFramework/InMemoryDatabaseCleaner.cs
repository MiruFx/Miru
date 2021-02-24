using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Databases;

namespace Miru.Tests.Databases.EntityFramework
{
    public class InMemoryDatabaseCleaner : IDatabaseCleaner
    {
        private readonly DbContext _db;

        public InMemoryDatabaseCleaner(DbContext db)
        {
            _db = db;
        }

        public async Task ClearAsync()
        {
            await _db.Database.EnsureDeletedAsync();
        }
    }
}