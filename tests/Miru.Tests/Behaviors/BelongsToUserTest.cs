using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.BelongsToUser;
using Miru.Databases;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Security;
using Miru.Tests.Databases.EntityFramework;
using Miru.Userfy;
using Z.EntityFramework.Plus;

namespace Miru.Tests.Behaviors;

public class BelongsToUserTest
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
        var user = _.Make<User>();
        _.Save(user);
        _.LoginAs(user);
            
        // act
        var post = _.Make<Post>(x => x.User = null);
        _.Save(post);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
        saved.UserId.ShouldBe(user.Id);
        saved.ShouldBe(post);
    }

    [Test]
    public void Should_not_set_if_user_is_already_set()
    {
        // arrange
        // no current user

        // act
        var post = _.Make<Post>(x =>
        {
            x.User = null;
            x.UserId = 10;
        });
        _.Save(post);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.AsNoFilter().First());
        saved.UserId.ShouldBe(10);
        saved.ShouldBe(post);
    }

    [Test]
    public void Should_not_set_if_user_id_is_already_set()
    {
        // arrange
        // no current user
            
        // act
        var post = _.Make<Post>();
        _.Save(post);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.AsNoFilter().First());
        saved.UserId.ShouldBe(post.User.Id);
        saved.ShouldBe(post);
    }
    
    [Test]
    public void Should_query_only_entities_belonged_to_current_user()
    {
        // arrange
        var user1 = _.Make<User>();
        var otherUser  = _.Make<User>();
        
        var post1 = _.Make<Post>(x => x.User = user1);
        var post2 = _.Make<Post>(x => x.User = user1);
        var postOtherUser = _.Make<Post>(x => x.User = otherUser);

        _.Save(user1, otherUser, post1, post2, postOtherUser);
        
        _.LoginAs(user1);
        
        // act
        var posts = _.App.WithScope(s => s.Get<FooDbContext>().Posts.ToList());
            
        // assert
        posts.ShouldCount(2);
        posts.ShouldContainAll(post1, post2);
    }

    [Test]
    public void Throw_exception_if_current_user_is_anonymous()
    {
        // arrange
        // act
        Should.Throw<UnauthorizedException>(() =>
        {
            var post = _.Make<Post>(x =>
            {
                x.User = null;
                x.UserId = 0;
            });
            
            _.Save(post);
        });
            
        // assert
        _.App.WithScope(s => s.Get<FooDbContext>().Posts.Count().ShouldBe(0));
    }

    public class User : UserfyUser
    {
        public override string Display => UserName;
    }
        
    public class Post : Entity, IBelongsToUser<User>
    {
        public long UserId { get; set; }
        public User User { get; set; }
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