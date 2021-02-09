using System.Linq;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;
using Miru.Userfy;

namespace Miru.Databases.EntityFramework
{
    public class BelongsToUserBeforeSaveHandler : IBeforeSaveHandler
    {
        private readonly IUserSession _userSession;

        public BelongsToUserBeforeSaveHandler(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public void BeforeSaveChanges(DbContext db)
        {
            if (_userSession.IsLogged == false)
                return;

            var entities = db.ChangeTracker.Entries<IBelongsToUser>()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);

            foreach (var entity in entities)
            {
                if (entity.UserId == 0)
                    entity.UserId = _userSession.CurrentUserId.ToLong();
            }
        }
    }
}