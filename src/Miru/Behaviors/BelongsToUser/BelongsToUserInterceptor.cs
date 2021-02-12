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
    public class BelongsToUserInterceptor : SaveChangesInterceptor
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
            var entities = @event.Context.ChangeTracker.Entries<IBelongsToUser>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);
            
            if (entities.Any())
                if (_currentUser.IsAnonymous)
                    throw new UnauthorizedException($"Authenticated user is required");
            
            foreach (var entity in entities)
            {
                if (entity.UserId == 0)
                    entity.UserId = _currentUser.Id;
            }
            
            return new ValueTask<InterceptionResult<int>>(result);
        }
    }
}