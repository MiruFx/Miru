using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.Inactivable;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Testing;
using Miru.Tests.Databases.EntityFramework;
using NUnit.Framework;
using Shouldly;
using Z.EntityFramework.Plus;

namespace Miru.Tests.Behaviors
{
    public class InactivableTest
    {
        private TestFixture _;
        
        [OneTimeSetUp]
        public void SetupFixture()
        {
            _ = new ServiceCollection()
                .AddMiruApp()
                .AddFeatureTesting()
                .AddMiruTestFixture()
                .AddEfCoreInMemory<FooDbContext>()
                .AddDatabaseCleaner<InMemoryDatabaseCleaner>()
                
                .AddInactivable()
                
                .BuildServiceProvider()
                .GetService<TestFixture>();    
        }

        [SetUp]
        public void Setup()
        {
            _.ClearDatabase();
        }

        [Test]
        public void Should_return_only_active_entities()
        {
            // arrange
            var activePost = new Post {Title = "Hello"};
            var inactivePost = new Post {Title = "Old Hello", IsInactive = true};
            
            // act
            _.Save(activePost, inactivePost);
            
            // assert
            var allPosts = _.App.WithScope(s => s.Get<FooDbContext>().Posts.ToList());
            allPosts.ShouldCount(1);
            allPosts.ShouldContain(activePost);
        }

        [Test]
        public void Should_return_inactives_when_use_no_filter()
        {
            // arrange
            var activePost = new Post {Title = "Hello"};
            var inactivePost = new Post {Title = "Old Hello", IsInactive = true};
            
            // act
            _.Save(activePost, inactivePost);
            
            // assert
            var allPosts = _.App.WithScope(s => s.Get<FooDbContext>().Posts.AsNoFilter().ToList());
            allPosts.ShouldCount(2);
            allPosts.ShouldContain(activePost);
            allPosts.ShouldContain(inactivePost);
        }

        public class Post : Entity, IInactivable
        {
            public string Title { get; set; }
            public bool IsInactive { get; set; }
        }

        public class FooDbContext : MiruDbContext
        {
            public FooDbContext(IMiruApp app) : base(app)
            {
            }
        
            public DbSet<Post> Posts { get; set; }
        }
    }
}