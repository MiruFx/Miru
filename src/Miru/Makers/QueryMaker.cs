using Miru.Core;

namespace Miru.Makers
{
    public static class QueryMaker
    {
        public static void Query(this Maker maker, string @in, string name, string action)
        {
            var input = new
            {
                Name = name, 
                In = maker.Namespace(@in),
                Action = action
            };
            
            maker.Template("Query", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}.cs");
            
            maker.Template("Query.cshtml", input, maker.Solution.FeaturesDir / @in / $"{action}.cshtml");
            
            maker.Template("QueryTest", input, maker.Solution.AppTestsDir / "Features" / @in / $"{name}{action}Test.cs");
            
            maker.Template("QueryPageTest", input, maker.Solution.AppPageTestsDir / "Pages" / @in / $"{name}{action}PageTest.cs");
        }
    }
}