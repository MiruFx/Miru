using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Tests.Databases.EntityFramework;
using Miru.Userfy;
using Z.EntityFramework.Plus;

namespace Miru.Tests.Behaviors;

public class InactivableTest : MiruCoreTesting
{
    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddEfCoreInMemory<FooDbContext>()
            .AddDatabaseCleaner<InMemoryDatabaseCleaner>()
            .AddFabrication()

            .AddUserfy<User, IdentityRole<long>, FooDbContext>(
                identity: cfg =>
                {
                    cfg.Password.RequiredLength = 3;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequireNonAlphanumeric = false;
                    cfg.Password.RequireLowercase = false;
            
                    cfg.User.RequireUniqueEmail = true;
                })
            .AddTestingUserSession<User>()
            
            // being tested
            .AddInactivable();
    }

    [SetUp]
    public void Setup()
    {
        _.ClearDatabase();
        _.ClearFabricator();
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
    
    [Test]
    public void If_object_is_active_then_should_inactivate()
    {
        // arrange
        var post = new Post { IsInactive = false };
            
        // act
        post.ActivateOrInactivate();
            
        // assert
        post.IsInactive.ShouldBeTrue();
        post.IsActive().ShouldBeFalse();
    }
    
    [Test]
    public void If_object_is_inactive_then_should_activate()
    {
        // arrange
        var post = new Post { IsInactive = true };
            
        // act
        post.ActivateOrInactivate();
            
        // assert
        post.IsInactive.ShouldBeFalse();
        post.IsActive().ShouldBeTrue();
    }

    [Test]
    public async Task If_user_is_active_then_should_login()
    {
        // arrange
        var user = await _.MakeUserAsync<User>(password: "123456");
       
        user.IsInactive = false;
       
        _.Save(user);
       
        // act
        var result = await _.Get<IUserLogin<User>>().LoginAsync(user.Email, "123456");
           
        // assert
        result.Succeeded.ShouldBeTrue();
    }
    
    [Test]
    public async Task If_user_is_inactive_then_should_not_login()
    {
        // arrange
        var user = await _.MakeUserAsync<User>(password: "123456");
        
        user.Inactivate();
        
        _.Save(user);
        
        // act
        var result = _.App.WithScope(s => s.Get<IUserLogin<User>>().LoginAsync(user.Email, "123456").Result);
            
        // assert
        result.Succeeded.ShouldBeFalse();
    }
    
    public class Post : Entity, IInactivable
    {
        public string Title { get; set; }
        public bool IsInactive { get; set; }
    }

    public class User : UserfyUser, IInactivable
    {
        public string Name { get; set; }
        public bool IsInactive { get; set; }
        public override string Display => Name;
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