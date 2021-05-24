using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Storages;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Storages
{
    public class StorageTest
    {
        public class DefaultStorage
        {
            private ServiceProvider _sp;
            private Storage _storage;
            private MiruSolution _solution;

            [OneTimeSetUp]
            public void Setup()
            {
                _solution = new MiruSolution(A.Path("Musicfy"));
                
                _sp = new ServiceCollection()
                    .AddMiruSolution(_solution)
                    .AddStorage()
                    .BuildServiceProvider();

                _ = _sp.GetService<ITestFixture>();

                _storage = _sp.GetRequiredService<Storage>();
            }

            [Test]
            public void Storage_dir_should_be_same_as_miru_solution_storage_dir()
            {
                var miruSolution = _sp.GetRequiredService<MiruSolution>();
                
                _storage.StorageDir.ShouldBe(miruSolution.StorageDir);
            }
            
            [Test]
            public void Temp_dir_inside_storage()
            {
                _storage.Temp().ShouldBe(A.Path("Musicfy") / "storage" / "temp");
            }
            
            [Test]
            public void App_dir_inside_storage()
            {
                _storage.App.ShouldBe(A.Path("Musicfy") / "storage" / "app");
            }
            
            [Test]
            public void Assets_dir_inside_storage()
            {
                _storage.Assets.ShouldBe(A.Path("Musicfy") / "storage" / "assets");
            }
        }

        public class CustomStorage
        {
            private ServiceProvider _sp;
            private MiruSolution _solution;
            private ThisStorage _storage;

            [OneTimeSetUp]
            public void Setup()
            {
                _solution = new MiruSolution(A.Path("MyApp"));
                
                _sp = new ServiceCollection()
                    .AddMiruSolution(_solution)
                    .AddStorage<ThisStorage>()
                    .BuildServiceProvider();

                _ = _sp.GetService<ITestFixture>();

                _storage = _sp.GetRequiredService<ThisStorage>();
            }

            [Test]
            public void Custom_dir_path()
            {
                _storage.ReportsDir.ShouldBe(_storage.App / "reports");
            }
            
            public class ThisStorage : Storage
            {
                public ThisStorage(MiruSolution solution) : base(solution)
                {
                }
                
                public MiruPath ReportsDir => App / "reports";
            }
        }

        public class TestStorage
        {
            private ServiceProvider _sp;
            private MiruSolution _solution;
            private Storage _storage;

            [OneTimeSetUp]
            public void Setup()
            {
                _solution = new MiruSolution(A.Path("MyApp"));
                
                _sp = new ServiceCollection()
                    .AddMiruSolution(_solution)
                    .AddTestStorage()
                    .BuildServiceProvider();

                _storage = _sp.GetRequiredService<Storage>();
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
}