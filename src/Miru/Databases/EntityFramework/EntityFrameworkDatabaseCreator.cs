using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Miru.Databases.EntityFramework
{
    public class EntityFrameworkDatabaseCreator : IDatabaseCreator
    {
        private readonly DbContext _db;

        public EntityFrameworkDatabaseCreator(DbContext db)
        {
            _db = db;
        }

        public async Task Create()
        {
            await _db.GetService<IRelationalDatabaseCreator>().CreateAsync();
        }
    }
}