using System.Linq;
using Microsoft.EntityFrameworkCore;
using Miru.Behaviors.BelongsToUser;
using Miru.Domain;
using Miru.Userfy;

namespace Miru.Databases.EntityFramework
{
    public class BelongsToUserBeforeSaveHandler : IBeforeSaveHandler
    {
        private readonly ICurrentUser _currentUser;

        public BelongsToUserBeforeSaveHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public void BeforeSaveChanges(DbContext db)
        {
            if (_currentUser.IsLogged == false)
                return;

            var entities = db.ChangeTracker.Entries<IBelongsToUser>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);

            foreach (var entity in entities)
            {
                if (entity.UserId == 0)
                    entity.UserId = _currentUser.Id.ToLong();
            }
        }
    }
}