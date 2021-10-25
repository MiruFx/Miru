using Miru.Core;

namespace Miru.Makers
{
    public static class FeatureMaker
    {
        public static void Feature(
            this Maker maker, 
            string @in, 
            string name, 
            string action, 
            string template,
            bool withTurboResult = true,
            bool withView = true,
            bool withFeatureTest = true,
            bool withPageTest = true)
        {
            var input = new
            {
                Name = name, 
                In = maker.Namespace(@in), 
                Action = action
            };
            
            maker.Template($"{template}-Feature", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{name}{action}.cs");
            
            if (withView)
                maker.Template($"{template}-Feature.cshtml", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"{action}.cshtml");
            
            if (withTurboResult && (template.EndsWith("New") || template.EndsWith("Edit")))
                maker.Template($"{template}-_Feature.turbo.cshtml", input, maker.Solution.FeaturesDir / maker.Expand(@in) / $"_{action}.turbo.cshtml");

            if (withFeatureTest)
                maker.Template($"{template}-FeatureTest", input, maker.Solution.AppTestsDir / "Features" / maker.Expand(@in) / $"{name}{action}Test.cs");
             
            if (withPageTest)
                maker.Template($"{template}-FeaturePageTest", input, maker.Solution.AppPageTestsDir / "Pages" / maker.Expand(@in) / $"{name}{action}PageTest.cs");
        }
    }
}