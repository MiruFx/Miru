using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;

namespace Miru.Databases.EntityFramework
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Add or Update an Entity. If the Id is 0 it adds, otherwise it updates
        /// </summary>
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