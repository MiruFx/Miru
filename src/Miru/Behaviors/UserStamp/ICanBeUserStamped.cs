using Miru.Domain;
using Miru.Userfy;

namespace Miru.Behaviors.UserStamp;

public interface ICanBeUserStamped : IEntity
{
    long? CreatedById { get; set; }
    long? UpdatedById { get; set; }
}

public interface ICanBeUserStamped<TUser> : ICanBeUserStamped where TUser : UserfyUser
{
    TUser CreatedBy { get; set; }
    TUser UpdatedBy { get; set; }
}