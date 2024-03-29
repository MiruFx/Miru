using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Storages;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Storages;

public class TestStorageTest
{
    private ServiceProvider _sp;
    private MiruSolution _solution;
    private IAppStorage _storage;

    [OneTimeSetUp]
    public void Setup()
    {
        _solution = new MiruSolution(A.Path / "MyApp");
                
        _sp = new ServiceCollection()
            .AddMiruSolution(_solution)
            
            // class under test
            .AddAppTestStorage()
            
            .BuildServiceProvider();

        _storage = _sp.GetRequiredService<IAppStorage>();
    }

    [Test]
    public void App_path_should_be_storage_test()
    {
        _storage.Path.ShouldBe(_solution.StorageDir / "tests" / "app");
    }
}