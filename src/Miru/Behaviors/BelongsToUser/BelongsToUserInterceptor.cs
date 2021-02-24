using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;
using Miru.Security;
using Miru.Userfy;

namespace Miru.Behaviors.BelongsToUser
{
    public class BelongsToUserInterceptor<TUser> : SaveChangesInterceptor
        where TUser : UserfyUser
    {
        private readonly ICurrentUser _currentUser;
        
        public BelongsToUserInterceptor(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData @event, 
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var entities = @event.Context.ChangeTracker.Entries<IBelongsToUser<TUser>>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);

            foreach (var entity in entities)
            {
                if (entity.UserId == 0 && entity.User == null)
                {
                    CheckNotAnonymous(entity);
                    
                    entity.UserId = _currentUser.Id;
                }
            }
            
            return new ValueTask<InterceptionResult<int>>(result);
        }

        private void CheckNotAnonymous(IBelongsToUser<TUser> belongsToUser)
        {
            if (_currentUser.IsAnonymous || _currentUser.Id == 0)
                throw new UnauthorizedException(
                    $"The entity {belongsToUser.GetType().Name} has {nameof(IBelongsToUser<TUser>)}. User should be set in the entity or Authenticated user is required");
        }
    }
}