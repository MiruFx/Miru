using Miru.Core;

namespace Miru.Makers
{
    public static class FeatureMaker
    {
        public static void Feature(this Maker maker, string @in, string name, string action, string template)
        {
            var input = new
            {
                Name = name, 
                In = maker.Namespace(@in), 
                Action = action
            };
            
            maker.Template($"{template}-Feature", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}.cs");
            
            maker.Template($"{template}-Feature.cshtml", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{action}.cshtml");
            
            if (template.EndsWith("New") || template.EndsWith("Edit"))
                maker.Template($"{template}-_Feature.js.cshtml", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"_{action}.js.cshtml");
            
            maker.Template($"{template}-FeatureTest", input, maker.Solution.AppTestsDir / "Features" / maker.Expand(@in) / $"{name}{action}Test.cs");
            
            maker.Template($"{template}-FeaturePageTest", input, maker.Solution.AppPageTestsDir / "Pages" / maker.Expand(@in) / $"{name}{action}PageTest.cs");
        }
    }
}