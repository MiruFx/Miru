using System.Collections;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;

namespace Miru.Databases.EntityFramework
{
    public class EntityFrameworkDataAccess : IDataAccess
    {
        private readonly DbContext _db;

        public EntityFrameworkDataAccess(DbContext db)
        {
            _db = db;
        }

        public void Persist(object[] entities)
        {
            using (var tx = _db.Database.BeginTransaction())
            {
                AddOrUpdateEntities(entities);

                _db.SaveChanges();
                
                tx.Commit();
            }
        }

        public async Task PersistAsync(object[] entities)
        {
            using (var tx = await _db.Database.BeginTransactionAsync())
            {
                AddOrUpdateEntities(entities);

                await _db.SaveChangesAsync();
                
                await tx.CommitAsync();
            }
        }

        private void AddOrUpdateEntities(object[] entities)
        {
            foreach (var entity in entities)
            {
                if (entity is IEnumerable collectionOfEntities)
                    foreach (var castEntity in collectionOfEntities)
                        Save(castEntity);
                else
                    Save(entity);
            }
        }

        private void Save(object entity)
        {
            if (entity is IEntity castEntity)
            {
                if (castEntity.IsNew())
                {
                    _db.Add(entity);
                }
                else
                {
                    var entry = _db.Entry(entity);
                
                    if (entry.State == EntityState.Detached)
                        _db.Attach(entity);
                
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
