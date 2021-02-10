using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class FeatureMakerTest
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
        public void Make_feature_for_new()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Feature("Goals", "Goal", "New", "New");
            
            (m.Solution.FeaturesDir / "Goals" / "GoalNew.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Goals" / "New.cshtml").ShouldExist();
            (m.Solution.FeaturesDir / "Goals" / "_New.js.cshtml").ShouldExist();
            
            (m.Solution.AppTestsDir / "Features" / "Goals" / "GoalNewTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Goals" / "GoalNewPageTest.cs").ShouldExist();
        }
        
        [Test]
        public void Make_feature_for_list()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Feature("Goals", "Goal", "List", "List");
            
            (m.Solution.FeaturesDir / "Goals" / "GoalList.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Goals" / "List.cshtml").ShouldContain(
                "<a href=\"#\">Edit</a>");
            
            (m.Solution.AppTestsDir / "Features" / "Goals" / "GoalListTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Goals" / "GoalListPageTest.cs").ShouldExist();
        }
        
        [Test]
        public void Make_feature_for_show()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.Feature("Goals", "Goal", "Details", template: "Show");
            
            // assert
            (m.Solution.FeaturesDir / "Goals" / "GoalDetails.cs").ShouldContain(
                "public class GoalDetails");
            
            (m.Solution.FeaturesDir / "Goals" / "Details.cshtml").ShouldContain(
                "<a href=\"#\">Edit</a>");
            
            (m.Solution.AppTestsDir / "Features" / "Goals" / "GoalDetailsTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Goals" / "GoalDetailsPageTest.cs").ShouldExist();
        }
    }
}