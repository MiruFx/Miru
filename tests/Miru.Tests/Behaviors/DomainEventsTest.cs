using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Pipeline;
using Miru.Testing;
using Miru.Tests.Databases.EntityFramework;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Behaviors
{
    public class DomainEventsTest
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
                .AddDefaultPipeline<DomainEventsTest>()
                
                // being tested
                .AddDomainEvents()
                
                .BuildServiceProvider()
                .GetService<TestFixture>();    
        }

        [SetUp]
        public void Setup()
        {
            _.ClearDatabase();
        }

        [Test]
        public void Should_publish_dispatch_and_handle_event()
        {
            // arrange
            var post = new Post("Original post");
            
            // act
            _.Save(post);
            
            // assert
            post.DomainEvents.ShouldBeEmpty();
            
            var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
            saved.Title.ShouldBe("Yeah! This will be the title");
        }
        
        [Test]
        public void Should_publish_event()
        {
            // arrange & act
            var post = new Post("Original post");
            
            // assert
            post.ShouldPublishEvent<PostCreated>();
        }

        public class Post : Entity
        {
            public Post()
            {
            }

            public Post(string title)
            {
                Title = title;
                
                PublishEvent(new PostCreated(this));
            }

            public string Title { get; set; }
        }

        public class PostCreated : IDomainEvent, INotification
        {
            public Post Post { get; }

            public PostCreated(Post post)
            {
                Post = post;
            }
        }

        public class PostSetTitle : INotificationHandler<PostCreated>
        {
            private readonly FooDbContext _db;

            public PostSetTitle(FooDbContext db)
            {
                _db = db;
            }

            public async Task Handle(PostCreated notification, CancellationToken ct)
            {
                notification.Post.Title = "Yeah! This will be the title";

                await _db.SaveChangesAsync(ct);
            }
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