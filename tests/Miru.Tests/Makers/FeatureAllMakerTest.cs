using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class FeatureAllMakerTest
    {
        private MiruPath _solutionDir;
        
        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath("Miru", "Mong");
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_feature_all()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.FeatureAll("Teams", "Team");
            
            (m.Solution.FeaturesDir / "Teams" / "TeamNew.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "New.cshtml").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "_New.turbo.cshtml").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamNewTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamNewPageTest.cs").ShouldExist();
            
            (m.Solution.FeaturesDir / "Teams" / "TeamEdit.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "Edit.cshtml").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "_Edit.turbo.cshtml").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamEditTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamEditPageTest.cs").ShouldExist();            

            (m.Solution.FeaturesDir / "Teams" / "TeamShow.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "Show.cshtml").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamShowTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamShowPageTest.cs").ShouldExist();  
            
            (m.Solution.FeaturesDir / "Teams" / "TeamList.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "List.cshtml").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamListTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamListPageTest.cs").ShouldExist();  
        }
    }
}