using Miru.Core;

namespace Miru.Makers
{
    public static class CrudMaker
    {
        public static void Crud(this Maker maker, string @in, string name)
        {
            maker.Feature(@in, name, "New", "Crud-New");
            maker.Feature(@in, name, "Edit", "Crud-Edit");
            maker.Feature(@in, name, "Show", "Crud-Show");
            maker.Feature(@in, name, "List", "Crud-List");
        }
    }
}