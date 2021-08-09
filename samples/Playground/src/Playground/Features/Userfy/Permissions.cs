using Miru.Domain;

namespace Playground.Features.Userfy
{
    public class Permissions : Enumeration<Permissions>
    {
        public static Permissions ProductList = new Permissions(1, "List Products");
        public static Permissions ProductEdit = new Permissions(2, "Edit Products");

        public Permissions(int value, string name) : base(value, name)
        {
        }
    }

    public class Permission 
    {
    }
}