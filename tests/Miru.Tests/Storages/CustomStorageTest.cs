using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Storages;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Storages
{
    public class CustomStorageTest
    {
        private ServiceProvider _sp;
        private MiruSolution _solution;
        private ThisStorage _storage;

        [OneTimeSetUp]
        public void Setup()
        {
            _solution = new MiruSolution(A.Path / "MyApp");
                
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
            
        public class ThisStorage : LocalDiskStorage
        {
            public ThisStorage(MiruSolution solution) : base(solution)
            {
            }
                
            public MiruPath ReportsDir => App / "reports";
        }
    }
}