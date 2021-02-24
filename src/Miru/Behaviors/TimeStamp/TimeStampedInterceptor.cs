using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;

namespace Miru.Behaviors.TimeStamp
{
    public class TimeStampedInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData @event, 
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var entitiesBeingCreated = @event.Context.ChangeTracker.Entries<ITimeStamped>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            foreach (var entityBeingCreated in entitiesBeingCreated)
            {
                if (entityBeingCreated.CreatedAt == default)
                    entityBeingCreated.CreatedAt = DateTime.Now;
                
                entityBeingCreated.UpdatedAt = DateTime.Now;
            }

            var entitiesBeingUpdated = @event.Context.ChangeTracker.Entries<ITimeStamped>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            foreach (var entityBeingUpdated in entitiesBeingUpdated)
            {
                entityBeingUpdated.UpdatedAt = DateTime.Now;
            }
            
            return new ValueTask<InterceptionResult<int>>(result);
        }
    }
}