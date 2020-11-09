using Miru.Domain;
using Miru.Userfy;

namespace Skeleton.Domain
{
    public class User : Entity, IUser, IHasPassword
    {
        public string Display => Email;

        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Name { get; set; }
    }
}