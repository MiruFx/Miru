using Microsoft.Extensions.DependencyInjection;
using Miru.Storages;

namespace Miru.Tests.Storages;

public class AppStorageTest
{
    private ServiceProvider _sp;
    private IAppStorage _storage;
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

        _storage = _sp.GetRequiredService<IAppStorage>();
    }
   
    [Test]
    public void Temp_dir_inside_storage()
    {
        _storage.Temp().ShouldBe(A.Path / "Musicfy" / "storage" / "app" / "temp");
    }
        
    [Test]
    public void App_dir_inside_storage()
    {
        _storage.Path.ShouldBe(A.Path / "Musicfy" / "storage" / "app");
    }
}