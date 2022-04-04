using System;
using System.Collections.Generic;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.TimeStamp;
using Miru.Databases;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Seeding;
using Miru.Tests.Databases.EntityFramework;
using Miru.Userfy;

namespace Miru.Tests.Seeding;

public class SeedingTest
{
    private TestFixture _;
        
    [OneTimeSetUp]
    public void SetupFixture()
    {
        _ = new ServiceCollection()
            .AddMiruApp()
            .AddFeatureTesting()
            .AddFabrication()
            .AddSingleton<TestFixture, TestFixture>()
            .AddEfCoreInMemory<FooDbContext>()
            .AddTimeStamp()
            .AddTestingUserSession<User>()
            .AddTransient<IDatabaseCleaner, InMemoryDatabaseCleaner>()
                
            .BuildServiceProvider()
            .GetService<TestFixture>();    
    }

    [SetUp]
    public void Setup()
    {
        _.Logout();
        _.ClearDatabase();
    }
    
    [Test]
    public void Should_add_new_entity()
    {
        // arrange
        
        // act
        _.WithDb<FooDbContext>(db =>
        {
            db.Products.SeedBy(x => x.Name, p =>
            {
                p.Name = "Shoes";
                p.Price = 10m;
            });

            db.SaveChanges();
        });
        
        // assert
        var savedProduct =  _.App.WithScope(s => s.Get<FooDbContext>().Products.Single());
        savedProduct.Name.ShouldBe("Shoes");
        savedProduct.Price.ShouldBe(10);
        savedProduct.CreatedAt.ShouldBeSecondsAgo();
        savedProduct.UpdatedAt.ShouldBeSecondsAgo();
    }

    [Test]
    public void If_entity_exist_should_update_entity_property_if_there_is_difference()
    {
        // arrange
        _.Save(new Product
        {
            Name = "Shoes B",
            Price = 10m,
            CreatedAt = 10.Days().Ago(),
            UpdatedAt = 10.Days().Ago()
        });
        
        // act
        _.WithDb<FooDbContext>(db =>
        {
            db.Products.SeedBy(x => x.Name, p =>
            {
                p.Name = "Shoes B";
                p.Price = 20m;
            });

            db.SaveChanges();
        });
        
        // assert
        var savedProduct =  _.App.WithScope(s => s.Get<FooDbContext>().Products.Single());
        savedProduct.Name.ShouldBe("Shoes B");
        savedProduct.Price.ShouldBe(20);
        savedProduct.CreatedAt.DateShouldBe(10.Days().Ago());
        savedProduct.UpdatedAt.ShouldBeSecondsAgo();
    }
    
    [Test]
    public void If_entity_exist_should_not_update_if_properties_are_the_same()
    {
        // arrange
        _.Save(new Product
        {
            Name = "Shoes C",
            Price = 10m,
            CreatedAt = 10.Days().Ago(),
            UpdatedAt = 10.Days().Ago()
        });
        
        // act
        _.WithDb<FooDbContext>(db =>
        {
            db.Products.SeedBy(x => x.Name, p =>
            {
                // same properties
                p.Name = "Shoes C";
                p.Price = 10m;
            });

            db.SaveChanges();
        });
        
        // assert
        var savedProduct =  _.App.WithScope(s => s.Get<FooDbContext>().Products.Single());
        savedProduct.Name.ShouldBe("Shoes C");
        savedProduct.Price.ShouldBe(10);
        savedProduct.CreatedAt.DateShouldBe(10.Days().Ago());
        savedProduct.UpdatedAt.DateShouldBe(10.Days().Ago());
    }
    
    public class Product : Entity, ITimeStamped
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
        
    public class FooDbContext : UserfyDbContext<User>
    {
        public FooDbContext(
            DbContextOptions options, 
            IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
        {
        }
        
        public DbSet<Product> Products { get; set; }
    }
}