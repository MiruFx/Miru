using Miru.Userfy;

namespace Miru.Domain
{
    public interface IBelongsToUser
    {
        long UserId { get; set; }
    }
    
    public interface IBelongsToUser<TUser> : IBelongsToUser where TUser : UserfyUser
    {
        TUser User { get; }
    }
}