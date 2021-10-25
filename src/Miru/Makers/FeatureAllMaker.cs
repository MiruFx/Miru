using Miru.Core;

namespace Miru.Makers
{
    public static class FeatureAllMaker
    {
        public static void FeatureAll(this Maker maker, string @in, string name)
        {
            maker.Feature(@in, name, "New", "Crud-New");
            maker.Feature(@in, name, "Edit", "Crud-Edit");
            maker.Feature(@in, name, "Show", "Crud-Delete");
            maker.Feature(@in, name, "List", "Crud-List");
        }
    }
}