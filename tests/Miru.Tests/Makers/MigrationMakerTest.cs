using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class MigrationMakerTest
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
        public void Make_migration()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Migration("CreateTopup", "123");
            
            (_solutionDir / "src" / "Shopifu" / "Database" / "Migrations" / "123_CreateTopup.cs")
                .ShouldExistAndContains(
                    "namespace Shopifu.Database.Migrations",
                    $"[Migration(123)]",
                    "public class CreateTopup");
        }
    }
}
