using Miru.Userfy;

namespace Supportreon.Domain
{
    public class User : UserfyUser, ICanBeAdmin
    {
        public string Name { get; set; }
        
        public override string Display => Name;
        
        public bool IsAdmin { get; set; }
    }
}
