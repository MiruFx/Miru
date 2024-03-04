using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Miru.Domain;

namespace Miru.Behaviors.Inactivable;

public static class InactivableDbSetExtensions
{
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