using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.UserStamp;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Testing;
using Miru.Tests.Databases.EntityFramework;
using Miru.Userfy;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Behaviors
{
    public class UserStampedTest
    {
        private TestFixture _;
        
        [OneTimeSetUp]
        public void SetupFixture()
        {
            _ = new ServiceCollection()
                .AddMiruApp()
                .AddFeatureTesting()
                .AddFabrication()
                .AddMiruTestFixture()
                .AddEfCoreInMemory<FooDbContext>()
                .AddDatabaseCleaner<InMemoryDatabaseCleaner>()
                .AddTestingUserSession<User>()
                
                .AddUserStamp()
                
                .BuildServiceProvider()
                .GetService<TestFixture>();    
        }

        [SetUp]
        public void Setup()
        {
            _.ClearDatabase();
        }

        [Test]
        public void Should_set_current_user_for_new_entity()
        {
            // arrange
            var user = _.MakeSavingLogin<User>();
            
            var post = new Post {Title = "Hello"};
            
            // act
            _.Save(post);
            
            // assert
            var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
            saved.CreatedById.ShouldBe(user.Id);
            saved.UpdatedById.ShouldBe(user.Id);
        }

        [Test]
        public void Should_update_current_user_for_existing_entity()
        {
            // arrange
            var otherUser = _.MakeSaving<User>();
            var currentUser = _.MakeSavingLogin<User>();
            
            var post = _.MakeSaving<Post>(x =>
            {
                x.CreatedById = otherUser.Id;
                x.UpdatedById = otherUser.Id;
            });
            
            // act
            _.Save(post);
            
            // assert
            var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
            saved.CreatedById.ShouldBe(otherUser.Id);
            saved.UpdatedById.ShouldBe(currentUser.Id);
        }

        public class Post : Entity, IUserStamped
        {
            public string Title { get; set; }
            
            public long CreatedById { get; set; }
            public long UpdatedById { get; set; }
        }

        public class User : UserfyUser
        {
            public override string Display => UserName;
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
            public DbSet<User> Users { get; set; }
            
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.AddInterceptors(_interceptors);
            }
        }
    }
}