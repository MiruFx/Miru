using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Storages;

namespace Miru.Tests.Storages;

public class AssetsStorageTest
{
    private ServiceProvider _sp;
    private IAssetsStorage _storage;
    private MiruSolution _solution;

    [OneTimeSetUp]
    public void Setup()
    {
        _solution = new MiruSolution(A.Path / "Musicfy");
            
        _sp = new ServiceCollection()
            .AddMiruSolution(_solution)
            
            // class under test
            .AddStorages()
            
            .BuildServiceProvider();

        _ = _sp.GetService<ITestFixture>();

        _storage = _sp.GetRequiredService<IAssetsStorage>();
    }

    [Test]
    public void Assets_dir_inside_storage()
    {
        _storage.Path.ShouldBe(_solution.StorageDir / "assets");
    }
}