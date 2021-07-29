using System.IO;
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
        private ServiceProvider _sp;
        private IStorage _storage;
        private MiruSolution _solution;

        [OneTimeSetUp]
        public void Setup()
        {
            _solution = new MiruSolution(A.Path / "Musicfy");
            
            _sp = new ServiceCollection()
                .AddMiruSolution(_solution)
                .AddStorage()
                .BuildServiceProvider();

            _ = _sp.GetService<ITestFixture>();

            _storage = _sp.GetRequiredService<IStorage>();
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
            _storage.Temp().ShouldBe(A.Path / "Musicfy" / "storage" / "temp");
        }
        
        [Test]
        public void App_dir_inside_storage()
        {
            _storage.App.ShouldBe(A.Path / "Musicfy" / "storage" / "app");
        }
        
        [Test]
        public void Assets_dir_inside_storage()
        {
            _storage.Assets.ShouldBe(A.Path / "Musicfy" / "storage" / "assets");
        }

        // [Test]
        // public void Should_put_existent_file()
        // {
        //     // arrange
        //     var existentFile = Path.GetTempFileName();
        //     File.WriteAllText(existentFile, nameof(Should_put_existent_file));
        //     
        //     // act
        //     _storage.Put(A.Root / "invoices" / "2021" / "invoice-1.txt", existentFile);
        //
        //     // assert
        //     File.ReadAllText(A.Root / "app" / "invoices" / "2021" /  "invoice-1.txt")
        //         .ShouldBe(nameof(Should_put_existent_file));
        // }
    }
}