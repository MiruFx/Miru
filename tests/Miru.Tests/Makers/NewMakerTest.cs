using System;
using System.IO;
using Miru.Core;
using Miru.Core.Makers;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Makers
{
    public class NewMakerTest
    {
        private MiruPath _tempDir;

        [SetUp]
        //[TearDown]
        public void Setup()
        {
            _tempDir = A.TempPath / "Miru";
            
            Directories.DeleteIfExists(_tempDir);
        }

        [Test]
        public void Make_new_solution()
        {
            var m = Maker.For(_tempDir, "StackOverflow");

            m.New("StackOverflow");

            // check some main files
            File.Exists(_tempDir / "StackOverflow" / ".gitignore").ShouldBeTrue();
            
            // config
            // (m.Solution.AppDir / "appSettings-example.yml").ShouldContain("{{ db_dir }}App_{{ environment }}");
            // (m.Solution.AppDir / "appSettings.Development.yml").ShouldContain("{{ db_dir }}App_Development");
            // (m.Solution.AppDir / "appSettings.Test.yml").ShouldContain("{{ db_dir }}App_Test");
        }
        
        [Test]
        public void Make_new_solution_named_with_dots()
        {
            var m = Maker.For(_tempDir, "StackExchange.StackOverflow");

            m.New("StackExchange.StackOverflow");

            (m.Solution.RootDir / ".gitignore").ShouldExist();
            
            (m.Solution.RootDir / "StackExchange.StackOverflow.sln")
                .ShouldContain(@"""StackExchange.StackOverflow"", ""src\StackExchange.StackOverflow\StackExchange.StackOverflow.csproj""");
            
            // config
            // (m.Solution.AppDir / "appSettings-example.yml").ShouldContain("{{ db_dir }}App_dev");
            // (m.Solution.AppDir / "appSettings.Development.yml").ShouldContain("{{ db_dir }}App_dev");
            // (m.Solution.AppDir / "appSettings.Test.yml").ShouldContain("{{ db_dir }}App_dev");
            
            // app
            (m.Solution.AppDir / "Database" / "StackOverflowDbContext.cs").ShouldContain("public class StackOverflowDbContext");
            
            // test
            (m.Solution.AppTestsDir / "StackOverflowFabricator.cs").ShouldContain("public class StackOverflowFabricator");
        }
        
        [Test]
        public void Cant_create_if_directory_already_exist()
        {
            Directories.CreateIfNotExists(_tempDir / "StackOverflow");
            
            var m = Maker.For(_tempDir);

            Should.Throw<MakeException>(() => m.New("StackOverflow"));
        }
    }
}