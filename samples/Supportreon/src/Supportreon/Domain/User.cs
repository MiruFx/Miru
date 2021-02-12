using Miru.Userfy;

namespace Supportreon.Domain
{
    public class User : UserfyUser
    {
        public string Name { get; set; }
        
        public override string Display => Name;
    }
}
