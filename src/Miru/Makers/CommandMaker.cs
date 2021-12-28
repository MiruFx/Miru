using Baseline;
using Miru.Core;

namespace Miru.Makers
{
    public static class CommandMaker
    {
        public static void Command(this Maker maker, string @in, string name, string action)
        {
            var input = new
            {
                Name = name,
                In = maker.Namespace(@in),
                UrlIn = maker.Url(@in),
                Action = action
            };
                
            maker.Template("Command", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}.cs");
            
            maker.Template("Command.cshtml", input, maker.Solution.FeaturesDir / @in / $"{action}.cshtml");
            
            maker.Template("_Command.turbo.cshtml", input, maker.Solution.FeaturesDir / @in / $"_{action}.turbo.cshtml");
            
            maker.Template("CommandTest", input, maker.Solution.AppTestsDir / "Features" / @in / $"{name}{action}Test.cs");
            
            maker.Template("CommandPageTest", input, maker.Solution.AppPageTestsDir / "Pages" / @in / $"{name}{action}PageTest.cs");
        }
    }
}