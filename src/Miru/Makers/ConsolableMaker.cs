using Miru.Core;

namespace Miru.Makers
{
    public static class ConsolableMaker
    {
        public static void Consolable(this Maker maker, string name)
        {
            var input = new
            {
                Name = name
            };
            
            maker.Template("Consolable", input, maker.Solution.AppDir / "Consolables" / $"{name}Consolable.cs");
        }
    }
}