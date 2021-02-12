using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.BelongsToUser;
using Miru.Databases;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Security;
using Miru.Testing;
using Miru.Userfy;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Databases.EntityFramework
{
    public class BelongsToUserInterceptorTest
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
                .AddTestingUserSession<User>()
                .AddTransient<IDatabaseCleaner, InMemoryDatabaseCleaner>()
                
                // being tested
                .AddBelongsToUser()
                
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
        public void Should_set_current_user()
        {
            // arrange
            var user = _.MakeSavingLogin<User>();
            
            // act
            var post = _.MakeSaving<Post>();
            
            // assert
            var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
            saved.UserId.ShouldBe(user.Id);
            saved.ShouldBe(post);
        }

        [Test]
        public void Throw_exception_if_current_user_is_anonymous()
        {
            // arrange
            // act
            Should.Throw<UnauthorizedException>(() => _.MakeSaving<Post>());
            
            // assert
            _.App.WithScope(s => s.Get<FooDbContext>().Posts.Count().ShouldBe(0));
        }

        public class User : UserfyUser
        {
            public override string Display => UserName;
        }
        
        public class Post : Entity, IBelongsToUser
        {
            public long UserId { get; set; }
            public string Title { get; set; }
        }

        public class FooDbContext : UserfyDbContext<User>
        {
            public FooDbContext(
                DbContextOptions options, 
                IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
            {
            }
        
            public DbSet<Post> Posts { get; set; }
        }
    }
}