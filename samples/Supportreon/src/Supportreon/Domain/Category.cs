using Miru.Domain;

namespace Supportreon.Domain
{
    public class Category : Entity, ILookupable
    {
        public string Name { get; set; }

        public string Value => Id.ToString();
        public string Display => Name;
    }
}
