using Miru.Userfy;

namespace Corpo.Skeleton.Domain;

public class User : UserfyUser
{
    public override string Display => Email;
}