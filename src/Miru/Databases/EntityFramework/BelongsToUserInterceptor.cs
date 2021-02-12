using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;
using Miru.Security;
using Miru.Userfy;

namespace Miru.Databases.EntityFramework
{
    public class BelongsToUserInterceptor : SaveChangesInterceptor
    {
        private readonly IUserSession _userSession;
        
        public BelongsToUserInterceptor(IUserSession userSession)
        {
            _userSession = userSession;
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
                if (_userSession.IsLogged == false)
                    throw new UnauthorizedException($"Authenticated user is required");
            
            foreach (var entity in entities)
            {
                if (entity.UserId == 0)
                    entity.UserId = _userSession.CurrentUserId;
            }
            
            return new ValueTask<InterceptionResult<int>>(result);
        }
    }
}