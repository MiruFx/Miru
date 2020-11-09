using Miru.Core;

namespace Miru.Makers
{
    public static class FabricatorMaker
    {
        public static void FabricatorFor(this Maker m, string name)
        {
            var input = new
            {
                name
            };
            
            m.Template("FabricatorFor", input, m.Solution.DatabaseDir / "Fabricators" / $"{name}Fabricator.cs");
        }
    }
}