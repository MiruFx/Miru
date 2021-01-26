using Humanizer;
using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class EntityMakerTest
    {
        private MiruPath _solutionDir;

        [SetUp]
        public void Setup()
        {
            _solutionDir = A.TempPath("Miru", "Mong");
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_entity()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Entity("Topup");
            
            (_solutionDir / "src" / "Mong" / "Domain" / "Topup.cs")
                .ShouldContain(
                    "public class Topup : Entity");
            
            (_solutionDir / "tests" / "Mong.Tests" / "Domain" / "TopupTest.cs")
                .ShouldContain(
                    "public class TopupTest : DomainTest");
        }
    }
}
