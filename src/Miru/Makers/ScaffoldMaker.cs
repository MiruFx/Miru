using Miru.Core;

namespace Miru.Makers
{
    public static class ScaffoldMaker
    {
        public static void Scaffold(this Maker maker, string @in, string name)
        {
            maker.Feature(@in, name, "New", "Crud-New", 
                withTurboResult: false, withView: false, withFeatureTest: false, withPageTest: false);
            
            maker.Feature(@in, name, "Edit", "Crud-Edit", 
                withTurboResult: false);
            
            maker.Feature(@in, name, "List", "Crud-List", 
                withTurboResult: false);
            
            maker.Feature(@in, name, "Delete", "Crud-Delete", 
                withTurboResult: false, withView: false, withPageTest: false);
        }
    }
}