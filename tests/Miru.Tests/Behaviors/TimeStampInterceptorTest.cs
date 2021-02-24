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
    public class TimeStampInterceptorTest
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
                .AddTimeStamped()
                
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
        
        //
        // [Test]
        // public void Should_not_set_if_user_id_is_already_set()
        // {
        //     // arrange
        //     // no current user
        //     
        //     // act
        //     var post = _.MakeSaving<Post>();
        //     
        //     // assert
        //     var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
        //     saved.UserId.ShouldBe(post.User.Id);
        //     saved.ShouldBe(post);
        // }
        //
        // [Test]
        // public void Throw_exception_if_current_user_is_anonymous()
        // {
        //     // arrange
        //     // act
        //     Should.Throw<UnauthorizedException>(() => _.MakeSaving<Post>(x =>
        //     {
        //         x.User = null;
        //         x.UserId = 0;
        //     }));
        //     
        //     // assert
        //     _.App.WithScope(s => s.Get<FooDbContext>().Posts.Count().ShouldBe(0));
        // }

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