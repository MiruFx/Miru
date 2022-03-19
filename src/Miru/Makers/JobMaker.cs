using Miru.Core;

namespace Miru.Makers;

public static class JobMaker
{
    public static void Job(
        this Maker maker, 
        string @in, 
        string name, 
        string action,
        bool onlyTests = false,
        bool noTests = false)
    {
        var input = new
        {
            Name = name, 
            In = maker.Namespace(@in),
            Action = action
        };
            
        if (onlyTests == false)
            maker.Template("Job", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}.cs");
            
        if (onlyTests || noTests == false)
            maker.Template("JobTest", input, maker.Solution.AppTestsDir / "Features" / @in / $"{name}{action}Test.cs");
    }
}