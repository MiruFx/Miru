using Miru.Consolables;
using Miru.Makers;

namespace Miru.Tests.Makers;

public class MakeJobConsolableTest
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
    public async Task Make_a_job()
    {
        // arrange
        var app = MiruHost
            .CreateMiruHost("miru", "make.job", "Orders", "Order", "Placed")
            .ConfigureServices(s =>
            {
                s.AddMiruSolution(A.TempPath / "Miru" / "Shopifu");
                s.AddConsolable<MakeJobConsolable>();
            })
            .BuildApp();

        // act
        await app.RunAsync();
        
        // assert
        var solution = app.Get<MiruSolution>();

        (solution.FeaturesDir / "Orders" / "OrderPlaced.cs")
            .ShouldContain(
                "namespace Shopifu.Features.Orders",
                "public class OrderPlaced");
            
        (solution.AppTestsDir / "Features" / "Orders" / "OrderPlacedTest.cs")
            .ShouldContain(
                "namespace Shopifu.Tests.Features.Orders",
                "public class OrderPlacedTest : FeatureTest",
                "public async Task Can_handle_placed_order_job()");
    }
    
    [Test]
    public async Task Make_only_job_test()
    {
        // arrange
        var app = MiruHost
            .CreateMiruHost("miru", "make.job", "Orders", "Order", "Placed", "--only-tests")
            .ConfigureServices(s =>
            {
                s.AddMiruSolution(A.TempPath / "Miru" / "Shopifu");
                s.AddConsolable<MakeJobConsolable>();
            })
            .BuildApp();
            
        // act
        await app.RunAsync();
            
        // assert
        var solution = app.Get<MiruSolution>();
        
        (solution.FeaturesDir / "Orders" / "OrderPlaced.cs").ShouldNotExist();
            
        (solution.AppTestsDir / "Features" / "Orders" / "OrderPlacedTest.cs")
            .ShouldContain(
                "namespace Shopifu.Tests.Features.Orders",
                "public class OrderPlacedTest : FeatureTest",
                "public async Task Can_handle_placed_order_job()");
    }
}