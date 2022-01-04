using System;
using System.Collections.Generic;
using System.Linq;
using Baseline.Dates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.TimeStamp;
using Miru.Databases;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Testing;
using Miru.Tests.Databases.EntityFramework;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Behaviors
{
    public class TimeStampTest
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
                .AddTransient<IDatabaseCleaner, InMemoryDatabaseCleaner>()
                
                // being tested
                .AddTimeStamp()
                
                .BuildServiceProvider()
                .GetService<TestFixture>();    
        }

        [SetUp]
        public void Setup()
        {
            _.ClearDatabase();
        }

        [Test]
        public void Should_set_current_timestamp_for_new_entity()
        {
            // arrange
            var post = new Post {Title = "Hello"};
            
            // act
            _.Save(post);
            
            // assert
            var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
            saved.CreatedAt.ShouldBeSecondsAgo();
            saved.UpdatedAt.ShouldBeSecondsAgo();
        }

        [Test]
        public void Should_update_current_timestamp_for_existing_entity()
        {
            // arrange
            var post = _.MakeSaving<Post>(x =>
            {
                x.CreatedAt = 1.Days().Ago();
                x.UpdatedAt = 1.Days().Ago();
            });
            
            // act
            _.Save(post);
            
            // assert
            var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
            saved.CreatedAt.ShouldBe(1.Days().Ago(), tolerance: 5.Seconds());
            saved.UpdatedAt.ShouldBeSecondsAgo();
        }
        
        [Test]
        public void Time_stamp_example()
        {
            // arrange
            var post = new Post();
            
            // act
            _.Save(post);
            
            // assert
            post.DumpToConsole();
            
            // Post: 
            //  CreatedAt: 2021-12-29T15:45:44.8259326+01:00
            //  UpdatedAt: 2021-12-29T15:45:44.8261866+01:00
            //  Id: 1
        }

        public class Post : Entity, ITimeStamped
        {
            public string Title { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

        public class FooDbContext : DbContext
        {
            private readonly IEnumerable<IInterceptor> _interceptors;
            
            public FooDbContext(
                DbContextOptions options, 
                IEnumerable<IInterceptor> interceptors) : base(options)
            {
                _interceptors = interceptors;
            }
        
            public DbSet<Post> Posts { get; set; }
            
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.AddInterceptors(_interceptors);
            }
        }
    }
}