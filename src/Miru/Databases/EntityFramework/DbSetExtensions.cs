using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Miru.Domain;

namespace Miru.Databases.EntityFramework
{
    public static class DbSetExtensions
    {
        public static async Task ByIdOrNewAsync<TEntity>(this DbContext db, TEntity entity) where TEntity : class
        {
            db.Set<TEntity>().Add(entity);
            await db.SaveChangesAsync();
        }
        
        public static async Task AddSavingAsync<TEntity>(this DbContext db, TEntity entity) where TEntity : class
        {
            db.Set<TEntity>().Add(entity);
            await db.SaveChangesAsync();
        }
        
        public static async Task AddOrUpdateAsync<TEntity>(
            this DbSet<TEntity> dbSet, 
            TEntity entity,
            CancellationToken ct) where TEntity : class, IEntity
        {
            if (entity.IsNew())
                await dbSet.AddAsync(entity, ct);
            else
                dbSet.Update(entity);
        }
        
        public static void AddIfNotExists<TEntity>(
            this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> predicate, TEntity entity) where TEntity : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();

            if (exists == false)
                dbSet.Add(entity);
        }
        
        public static async Task<EntityEntry<TEntity>> InactiveByIdAsync<TEntity>(
            this DbSet<TEntity> dbSet, 
            long id, 
            CancellationToken ct = default)
            where TEntity : class, IInactivable, IEntity 
        {
            var entity = await dbSet.ByIdOrFailAsync(id, ct);

            entity.IsInactive = true;
            
            return dbSet.Update(entity);
        }
    }
}