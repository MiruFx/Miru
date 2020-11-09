using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    // public class FabricatorMakerTest
    // {
    //     private MiruPath _solutionDir;
    //
    //     [SetUp]
    //     [TearDown]
    //     public void Setup()
    //     {
    //         _solutionDir = A.TempPath("Miru", "Shopifu");
    //         
    //         Directories.DeleteIfExists(_solutionDir);
    //     }
    //     
    //     [Test]
    //     public void Make_fabricator()
    //     {
    //         var m = new Maker(new MiruSolution(_solutionDir));
    //         
    //         m.FabricatorFor("Topup");
    //         
    //         (m.Solution.DatabaseDir / "Fabricators" / "TopupFabricator.cs")
    //             .ShouldExistAndContains(
    //                 "Shopifu.Database.Fabricators",
    //                 "public class TopupFabricator : FabricatorFor<Topup>",
    //                 "public TopupFabricator()");
    //     }
    // }
}
