using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Storages;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Storages
{
    public class TestStorageTest
    {
        private ServiceProvider _sp;
        private MiruSolution _solution;
        private IStorage _storage;

        [OneTimeSetUp]
        public void Setup()
        {
            _solution = new MiruSolution(A.Path / "MyApp");
                
            _sp = new ServiceCollection()
                .AddMiruSolution(_solution)
                .AddTestStorage()
                .BuildServiceProvider();

            _storage = _sp.GetRequiredService<IStorage>();
        }

        [Test]
        public void App_path_should_be_storage_test()
        {
            _storage.App.ShouldBe(_solution.StorageDir / "tests" / "app");
        }
            
        [Test]
        public void Assets_path_should_be_from_storage()
        {
            _storage.Assets.ShouldBe(_solution.StorageDir / "assets");
        }
    }
}