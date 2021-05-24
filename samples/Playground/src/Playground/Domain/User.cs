using Miru.Userfy;

namespace Playground.Domain
{
    public class User : UserfyUser
    {
        public override string Display => Email;
    }
}
