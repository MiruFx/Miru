using Miru.Core;

namespace Miru.Makers
{
    public static class JobMaker
    {
        public static void Job(this Maker maker, string @in, string name, string action)
        {
            var input = new
            {
                Name = name, 
                In = maker.Namespace(@in),
                Action = action
            };
            
            maker.Template("Job", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}.cs");
            
            maker.Template("JobTest", input, maker.Solution.AppTestsDir / "Features" / @in / $"{name}{action}Test.cs");
        }
    }
}