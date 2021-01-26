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
        [TearDown]
        public void Setup()
        {
            _tempDir = A.TempPath("Miru");
            
            Directories.DeleteIfExists(_tempDir);
        }

        [Test]
        public void Make_new_solution()
        {
            var m = Maker.For(_tempDir, "StackOverflow");

            m.New("StackOverflow");

            // check some main files
            File.Exists(_tempDir / "StackOverflow" / ".gitignore").ShouldBeTrue();
            File.Exists(_tempDir / "StackOverflow" / "global.json").ShouldBeTrue();
            
            File.ReadAllText(_tempDir / "StackOverflow" / "config" / "Config.Development.yml").ShouldContain("{{ db_dir }}StackOverflow_dev");
        }
        
        [Test]
        public void Make_new_solution_named_with_dots()
        {
            var m = Maker.For(_tempDir, "StackExchange.StackOverflow");

            m.New("StackExchange.StackOverflow");

            (m.Solution.RootDir / ".gitignore").ShouldExist();
            (m.Solution.RootDir / "global.json").ShouldExist();
            
            (m.Solution.RootDir / "config" / "Config.Development.yml").ShouldContain("{{ db_dir }}StackOverflow_dev");
        }
        
        [Test]
        public void Cant_create_if_directory_already_exist()
        {
            Directories.CreateIfNotExists(_tempDir / "StackOverflow");
            
            var m = Maker.For(_tempDir);

            Should.Throw<InvalidOperationException>(() => m.New("StackOverflow"));
        }
    }
}