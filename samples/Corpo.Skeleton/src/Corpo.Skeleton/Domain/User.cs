using Miru.Behaviors.TimeStamp;
using Miru.Userfy;

namespace Corpo.Skeleton.Domain;

public class User : UserfyUser, ITimeStamped
{
    public override string Display => Email;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}