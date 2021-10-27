using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class ScaffoldMakerTest
    {
        private MiruPath _solutionDir;
        
        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath / "Miru" / "Mong";
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_scaffold()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Scaffold("Teams", "Team");
            
            (m.Solution.FeaturesDir / "Teams" / "TeamNew.cs").ShouldExist();
            
            (m.Solution.FeaturesDir / "Teams" / "TeamEdit.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "Edit.cshtml").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamEditTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamEditPageTest.cs").ShouldExist();            

            (m.Solution.FeaturesDir / "Teams" / "TeamDelete.cs").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamDeleteTest.cs").ShouldExist();
            // (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamDeletePageTest.cs").ShouldExist();    
            
            (m.Solution.FeaturesDir / "Teams" / "TeamList.cs").ShouldExist();
            (m.Solution.FeaturesDir / "Teams" / "List.cshtml").ShouldExist();
            (m.Solution.AppTestsDir / "Features" / "Teams" / "TeamListTest.cs").ShouldExist();
            (m.Solution.AppPageTestsDir / "Pages" / "Teams" / "TeamListPageTest.cs").ShouldExist();  
        }
    }
}