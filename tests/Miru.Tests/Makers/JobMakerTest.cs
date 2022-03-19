using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers;

public class JobMakerTest
{
    private MiruPath _solutionDir;

    [SetUp]
    [TearDown]
    public void Setup()
    {
        _solutionDir = A.TempPath / "Miru" / "Shopifu";
            
        Directories.DeleteIfExists(_solutionDir);
    }
        
    [Test]
    public void Make_a_job()
    {
        // arrange
        var m = new Maker(new MiruSolution(_solutionDir));
            
        // act
        m.Job("Orders", "Order", "Placed");
            
        // assert
        (m.Solution.FeaturesDir / "Orders" / "OrderPlaced.cs")
            .ShouldContain(
                "namespace Shopifu.Features.Orders",
                "public class OrderPlaced");
            
        (m.Solution.AppTestsDir / "Features" / "Orders" / "OrderPlacedTest.cs")
            .ShouldContain(
                "namespace Shopifu.Tests.Features.Orders",
                "public class OrderPlacedTest : FeatureTest",
                "public async Task Can_handle_placed_order_job()");
    }
    
    [Test]
    public void Make_only_job_test()
    {
        // arrange
        var m = new Maker(new MiruSolution(_solutionDir));
            
        // act
        m.Job("Orders", "Order", "Placed", onlyTests: true);
            
        // assert
        (m.Solution.FeaturesDir / "Orders" / "OrderPlaced.cs").ShouldNotExist();
            
        (m.Solution.AppTestsDir / "Features" / "Orders" / "OrderPlacedTest.cs")
            .ShouldContain(
                "namespace Shopifu.Tests.Features.Orders",
                "public class OrderPlacedTest : FeatureTest",
                "public async Task Can_handle_placed_order_job()");
    }
}