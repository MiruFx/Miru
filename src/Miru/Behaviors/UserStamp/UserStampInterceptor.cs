using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;
using Miru.Userfy;

namespace Miru.Behaviors.UserStamp;

public class UserStampInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData @event, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (@event.Context is null) 
            return new ValueTask<InterceptionResult<int>>(result);
        
        HandleStamped(@event);
        HandleCanBeStamped(@event);

        return new ValueTask<InterceptionResult<int>>(result);
    }

    private void HandleStamped(DbContextEventData @event)
    {
        var entitiesBeingCreated = @event.Context!.ChangeTracker.Entries<IUserStamped>()
            .Where(p => p.State == EntityState.Added)
            .Select(p => p.Entity);

        foreach (var entityBeingCreated in entitiesBeingCreated)
        {
            if (entityBeingCreated.CreatedById == default)
                entityBeingCreated.CreatedById = currentUser.Id;

            if (entityBeingCreated.UpdatedById == default)
                entityBeingCreated.UpdatedById = currentUser.Id;
        }

        var entitiesBeingUpdated = @event.Context.ChangeTracker.Entries<IUserStamped>()
            .Where(p => p.State == EntityState.Modified)
            .Select(p => p.Entity);

        foreach (var entityBeingUpdated in entitiesBeingUpdated)
        {
            entityBeingUpdated.UpdatedById = currentUser.Id;
        }
    }
        
    private void HandleCanBeStamped(DbContextEventData @event)
    {
        if (currentUser.IsAuthenticated)
        {
            var entitiesBeingCreated = @event.Context!.ChangeTracker.Entries<ICanBeUserStamped>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            foreach (var entityBeingCreated in entitiesBeingCreated)
            {
                if (entityBeingCreated.CreatedById.HasValue == false)
                    entityBeingCreated.CreatedById = currentUser.Id;

                if (entityBeingCreated.UpdatedById.HasValue == false)
                    entityBeingCreated.UpdatedById = currentUser.Id;
            }
        }

        var entitiesBeingUpdated = @event.Context!.ChangeTracker.Entries<ICanBeUserStamped>()
            .Where(p => p.State == EntityState.Modified)
            .Select(p => p.Entity);

        foreach (var entityBeingUpdated in entitiesBeingUpdated)
        {
            // if we're updating entity and there is not user authenticated, 
            // the default behavior will be to set the UpdatedById to null, as 
            // unknown user has updated the entity
            entityBeingUpdated.UpdatedById = currentUser.IsAuthenticated 
                ? currentUser.Id
                : null;
        }
    }
}