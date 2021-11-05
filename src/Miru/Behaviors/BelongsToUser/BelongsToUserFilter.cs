using System.Linq;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;
using Miru.Userfy;
using Z.EntityFramework.Plus;

namespace Miru.Behaviors.BelongsToUser;

public class BelongsToUserFilter : IQueryFilter
{
    private readonly ICurrentUser _currentUser;

    public BelongsToUserFilter(ICurrentUser currentUser) => _currentUser = currentUser;

    public void Apply(DbContext db)
    {
        db.Filter<IBelongsToUser>(q => q.Where(x => x.UserId == _currentUser.Id));
    }
}