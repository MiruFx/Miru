using Miru.Core;
using Miru.Core.Makers;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Foundation
{
    // TODO: Split SolutionFinderTest and ConfigFinderTest
    public class SolutionAndConfigFinderTest
    {
        private readonly SolutionFinder _solutionFinder = new SolutionFinder();
        private readonly ConfigYmlFinder _configYmlFinder = new ConfigYmlFinder();
        private MiruPath _tempDir;
        private MiruPath _solutionDir;
        private MiruSolution _solution;

        [OneTimeSetUp]
        public void Setup()
        {
            _tempDir = A.TempPath("Miru", "SolutionFinderTest");
            
            Directories.DeleteIfExists(_tempDir);
            
            var maker = Maker.For(_tempDir, "Shoppers");
            
            maker.New("Shoppers");

            _solutionDir = _tempDir / "Shoppers";

            _solution = _solutionFinder.FromDir(_solutionDir).Solution;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directories.DeleteIfExists(_tempDir);
        }
        
        [Test]
        public void Find_sln_from_current_dir()
        {
            _solutionFinder.FindSolutionFrom(_solutionDir).ShouldBe(_solutionDir / "Shoppers.sln");
            
            var solution = _solutionFinder.FromDir(_solutionDir).Solution;
            solution.Name.ShouldBe("Shoppers");
            solution.RootDir.ShouldBe(_solutionDir);
        }
        
        [Test]
        public void Find_sln_from_src_dir()
        {
            _solutionFinder.FindSolutionFrom(_solutionDir / "src").ShouldBe(_solutionDir / "Shoppers.sln");
            
            var solution = _solutionFinder.FromDir(_solutionDir).Solution;
            solution.Name.ShouldBe("Shoppers");
            solution.RootDir.ShouldBe(_solutionDir);
        }
        
        [Test]
        public void Find_sln_from_project_dir()
        {
            _solutionFinder.FindSolutionFrom(_solutionDir / "tests" / "Shoppers.Tests").ShouldBe(_solutionDir / "Shoppers.sln");
            
            var solution = _solutionFinder.FromDir(_solutionDir).Solution;
            solution.Name.ShouldBe("Shoppers");
            solution.RootDir.ShouldBe(_solutionDir);
        }
        
        [Test]
        public void Find_sln_from_bin_output_dir()
        {
            var currentDir = _solutionDir / "tests" / "Shoppers.Tests" / "bin" / "Debug" / "netcoreapp2.1";
            
            _solutionFinder
                .FindSolutionFrom(currentDir)
                .ShouldBe(_solutionDir / "Shoppers.sln");
            
            var solution = _solutionFinder.FromDir(_solutionDir).Solution;
            solution.Name.ShouldBe("Shoppers");
            solution.RootDir.ShouldBe(_solutionDir);
        }

        [Test]
        public void Find_config_yml_at_config_dir_from_output_dir()
        {
            var configYmlFound = _configYmlFinder.FindConfigYmlFromDir(
                _solutionDir / "src" / "Shoppers" / "bin" / "Debug" / "netcoreapp2.2", 
                "Development",
                _solution);
                
            configYmlFound.ShouldBe(_solutionDir / "config" / "Config.Development.yml");
        }
        
        [Test]
        public void Find_config_yml_at_config_dir_from_root_dir()
        {
            var configYmlFound = _configYmlFinder.FindConfigYmlFromDir(
                _solutionDir, 
                "Test",
                _solution);
                
            configYmlFound.ShouldBe(_solutionDir / "config" / "Config.Test.yml");
        }
        
        [Test]
        public void Find_config_yml_at_config_dir_from_project_dir_with_no_custom_config_yml()
        {
            var configYmlFound = _configYmlFinder.FindConfigYmlFromDir(
                _solutionDir / "src" / "Shoppers", 
                "Staging",
                _solution);
                
            configYmlFound.ShouldBe(_solutionDir / "config" / "Config.Staging.yml");
        }
        
        [Test]
        public void Find_config_yml_at_project_dir_from_output_dir()
        {
            Files.Create(_solutionDir / "src" / "Mong" / "Config.Development.yml", "...");
            
            var configYmlFound = _configYmlFinder.FindConfigYmlFromDir(
                _solutionDir / "src" / "Mong" / "bin" / "Debug" / "netcoreapp2.2", 
                "Development",
                _solution);
                
            configYmlFound.ShouldBe(_solutionDir / "src" / "Mong" / "Config.Development.yml");
        }
    }
}