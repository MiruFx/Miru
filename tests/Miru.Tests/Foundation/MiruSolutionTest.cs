using Miru.Core;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Foundation
{
    public class MiruSolutionTest
    {
        [Test]
        public void Can_create_solution_info()
        {
            var solution = new MiruSolution(A.Path("Projects", "StackOverflow"));

            solution.Name.ShouldBe("StackOverflow");
            solution.RootDir.ShouldBe(A.Path("Projects", "StackOverflow"));
            solution.SrcDir.ShouldBe(A.Path("Projects", "StackOverflow", "src"));
            solution.ConfigDir.ShouldBe(A.Path("Projects", "StackOverflow", "config"));
            solution.TestsDir.ShouldBe(A.Path("Projects", "StackOverflow", "tests"));
            
            solution.App.ShouldBe("StackOverflow");
            solution.AppDir.ShouldBe(A.Path("Projects", "StackOverflow", "src", "StackOverflow"));
            
            solution.AppTests.ShouldBe("StackOverflow.Tests");
            solution.AppTestsDir.ShouldBe(A.Path("Projects", "StackOverflow", "tests", "StackOverflow.Tests"));
            
            solution.AppPageTests.ShouldBe("StackOverflow.PageTests");
            solution.AppPageTestsDir.ShouldBe(A.Path("Projects", "StackOverflow", "tests", "StackOverflow.PageTests"));
            
            solution.DatabaseDir.ShouldBe(A.Path("Projects", "StackOverflow", "src", "StackOverflow", "Database"));
            solution.MigrationsDir.ShouldBe(A.Path("Projects", "StackOverflow", "src", "StackOverflow", "Database", "Migrations"));
            solution.FabricatorsDir.ShouldBe(A.Path("Projects", "StackOverflow", "src", "StackOverflow", "Database", "Fabricators"));
        }

        [Test]
        public void Get_app_name_from_root_dir()
        {
            var solution = new MiruSolution(A.TempPath("Miru", "ContosoUniversity"));
            solution.Name.ShouldBe("ContosoUniversity");
        }
        
        [Test]
        public void Get_app_name_from_parameter()
        {
            var solution = new MiruSolution(A.TempPath("Miru", "ContosoUniversity"), "ContosoWeb");
            solution.Name.ShouldBe("ContosoWeb");
        }
        
        [Test]
        public void Get_relative_path()
        {
            var solution = new MiruSolution(A.TempPath("Miru", "ContosoUniversity"), "ContosoWeb");
            solution.Relative(m => m.ConfigDir).ShouldBe("config");
        }
        
        // [Test]
        // public void Whole_structure()
        // {
        //     // var solution = new Solution(A.Dir("D:", "Projects", "Shoppers"));
        //     var solution = Solution.Find(currentDir: A.Dir("D:", "Projects", "Shoppers"));
        //     
        //     solution.CurrentDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers"));
        //     solution.SrcDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers", "src"));
        //     solution.CurrentProject.ShouldBeEmpty();
        //     solution.RootDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers"));
        //     solution.ConfigDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers", "src", "Config"));
        //     
        //     solution.Projects.App.ShouldBe("Shoppers");
        //     solution.Projects.AppDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers", "src", "Shoppers"));
        //     
        //     solution.Projects.Tests.ShouldBe("Shoppers.Tests");
        //     solution.Projects.TestsDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers", "src", "Shoppers.Tests"));
        //     
        //     solution.Projects.PageTests.ShouldBe("Shoppers.PageTests");
        //     solution.Projects.PageTestsDir.ShouldBe(A.Dir("D:", "Projects", "Shoppers", "src", "Shoppers.PageTests"));
        // }
        
        // [Test]
        // public void Find_root()
        // {
        //     new Solution(currentDir: @"D:\Projects\Shoppers\src\Shoppers.Tests")
        //         .RootDir
        //         .ShouldBe(@"D:\Projects\Shoppers");
        //     
        //     new Solution(@"C:\Projects\Shoppers\src\Web\bin\Debug\netcoreapp2.1")
        //         .RootDir
        //         .ShouldBe(@"C:\Projects\Shoppers");
        //         
        //     new Solution(@"C:\Projects\Shoppers\src\Web")
        //         .RootDir
        //         .ShouldBe(@"C:\Projects\Shoppers");
        //         
        //     new Solution(@"C:\Projects\Shoppers\src")
        //         .RootDir
        //         .ShouldBe(@"C:\Projects\Shoppers");
        // }
        //
        // [Test]
        // public void Find_root_at_root_folder()
        // {
        //     // arrange
        //     _fs.DirectoryExists(@"D:\Projects\Shoppers\src").Returns(true);
        //     _fs.FileExists(@"D:\Projects\Shoppers\src\global.json").Returns(true);
        //     
        //     // asserts
        //     new Solution(@"D:\Projects\Shoppers", _fs)
        //         .RootDir
        //         .ShouldBe(@"D:\Projects\Shoppers");
        // }
        //
        // [Test]
        // public void Find_src_from_different_paths()
        // {
        //     // arrange
        //     _fs.DirectoryExists(@"D:\Projects\Shoppers\src").Returns(true);
        //     _fs.FileExists(@"D:\Projects\Shoppers\src\global.json").Returns(true);
        //     
        //     // asserts
        //     new Solution(@"D:\Projects\Shoppers", _fs)
        //         .SrcDir
        //         .ShouldBe(@"D:\Projects\Shoppers\src");
        //     
        //     new Solution(@"D:\Projects\Shoppers\src")
        //         .SrcDir
        //         .ShouldBe(@"D:\Projects\Shoppers\src");
        //     
        //     new Solution(@"D:\Projects\Shoppers\src\Shoppers.Tests")
        //         .SrcDir
        //         .ShouldBe(@"D:\Projects\Shoppers\src");
        // }
        //
        // [Test]
        // public void Find_current_project()
        // {
        //     Assert.Fail(); // solution.CurrentProject
        // }
        //
        // [Test]
        // public void Find_config_from_different_paths()
        // {
        //     // arrange
        //     _fs.DirectoryExists(@"D:\Projects\Shoppers\src").Returns(true);
        //     _fs.FileExists(@"D:\Projects\Shoppers\src\global.json").Returns(true);
        //     
        //     // asserts
        //     new Solution(@"D:\Projects\Shoppers", _fs)
        //         .ConfigDir
        //         .ShouldBe(@"D:\Projects\Shoppers\src\Config");
        //     
        //     new Solution(@"D:\Projects\Shoppers\src")
        //         .ConfigDir
        //         .ShouldBe(@"D:\Projects\Shoppers\src\Config");
        //     
        //     new Solution(@"D:\Projects\Shoppers\src\Shoppers.Tests")
        //         .ConfigDir
        //         .ShouldBe(@"D:\Projects\Shoppers\src\Config");
        // }
        //
        // [Test]
        // [Ignore("Going to change for miru.yml on the root of the project")]
        // public void Get_projects_info_from_global_json()
        // {
        //     var solution = new Solution().Projects;
        //     solution.App.ShouldBe("Shoppers");
        //     solution.Tests.ShouldBe("Shoppers.Tests");
        // }
        //
        // [Test]
        // public void Get_project_path()
        // {
        //     new Solution(@"D:\Projects\Shoppers\src\Shoppers.Tests")
        //         .Project("Shoppers")
        //         .ShouldBe(@"D:\Projects\Shoppers\src\Shoppers");
        // }
        //
        // [Test]
        // public void Cant_find_root()
        // {
        //     Should.Throw<InvalidOperationException>(() =>
        //     {
        //         new Solution(@"D:\Projects\MyRailsBlog");
        //     });
        // }
    }
}