using Miru.Userfy;

namespace MiruNext.Domain
{
    public class User : UserfyUser
    {
        public override string Display => Email;
    }
}
