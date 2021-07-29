using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;
using Miru.Userfy;

namespace Miru.Behaviors.UserStamp
{
    public class UserStampInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUser _currentUser;

        public UserStampInterceptor(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData @event, 
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var entitiesBeingCreated = @event.Context.ChangeTracker.Entries<IUserStamped>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            foreach (var entityBeingCreated in entitiesBeingCreated)
            {
                if (entityBeingCreated.CreatedById == default)
                    entityBeingCreated.CreatedById = _currentUser.Id;
                
                if (entityBeingCreated.UpdatedById == default)
                    entityBeingCreated.UpdatedById = _currentUser.Id;
            }

            var entitiesBeingUpdated = @event.Context.ChangeTracker.Entries<IUserStamped>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            foreach (var entityBeingUpdated in entitiesBeingUpdated)
            {
                entityBeingUpdated.UpdatedById = _currentUser.Id;
            }
            
            return new ValueTask<InterceptionResult<int>>(result);
        }
    }
}