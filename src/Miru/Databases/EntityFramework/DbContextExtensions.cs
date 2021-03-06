using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;

namespace Miru.Databases.EntityFramework
{
    public static class DbContextExtensions
    {
        public static async Task SaveOrUpdate<TEntity>(
            this DbContext db, 
            TEntity entity, 
            CancellationToken ct = default) where TEntity : class, IEntity
        {
            if (entity.IsNew())
                await db.AddSavingAsync(entity);
        }
        
        public static async Task AddOrUpdateAsync<TEntity>(
            this DbContext db, TEntity entity, CancellationToken ct = default) where TEntity : IEntity
        {
            if (entity.IsNew())
                await db.AddAsync(entity, ct);
            else
                db.Update(entity);
        }
    }
}