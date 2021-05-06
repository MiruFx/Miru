using Miru.Userfy;
// ReSharper disable All

namespace Supportreon.Domain
{
    public class User : UserfyUser, ICanBeAdmin
    {
        public string Name { get; set; }
        
        public override string Display => Name;
        
        public bool IsAdmin { get; set; }
        
        public string Culture { get; set; }
        
        public string Currency { get; set; }
        
        public string Language { get; set; }
    }
}
