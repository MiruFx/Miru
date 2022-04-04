using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Miru.Behaviors.TimeStamp;

public class TimeStampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        var entitiesBeingCreated = eventData.Context.ChangeTracker.Entries<ITimeStamped>()
            .Where(p => p.State == EntityState.Added)
            .Select(p => p.Entity);

        foreach (var entityBeingCreated in entitiesBeingCreated)
        {
            if (entityBeingCreated.CreatedAt == default)
                entityBeingCreated.CreatedAt = DateTime.Now;
                
            if (entityBeingCreated.UpdatedAt == default)
                entityBeingCreated.UpdatedAt = DateTime.Now;
        }

        var entitiesBeingUpdated = @eventData.Context.ChangeTracker.Entries<ITimeStamped>()
            .Where(p => p.State == EntityState.Modified)
            .Select(p => p.Entity);

        foreach (var entityBeingUpdated in entitiesBeingUpdated)
        {
            entityBeingUpdated.UpdatedAt = DateTime.Now;
        }
            
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        return new ValueTask<InterceptionResult<int>>(SavingChanges(eventData, result));
    }
}