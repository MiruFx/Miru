using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class ConsolableMakerTest
    {
        private MiruPath _solutionDir;

        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath("Miru", "Shopifu");
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_a_consolable()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.Consolable("Backup");
            
            // assert
            (m.Solution.AppDir / "Consolables" / "BackupConsolable.cs")
                .ShouldContain(
                    "namespace Shopifu.Consolables",
                    "public class BackupConsolable");
        }
    }
}