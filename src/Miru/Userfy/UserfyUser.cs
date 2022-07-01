using Microsoft.AspNetCore.Identity;
using Miru.Domain;

namespace Miru.Userfy;

public abstract class UserfyUser : IdentityUser<long>, IEntity
{
    public abstract string Display { get; }
}