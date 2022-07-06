using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.Inactivable;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Tests.Databases.EntityFramework;
using Z.EntityFramework.Plus;

namespace Miru.Tests.Behaviors;

public class InactivableTest : MiruCoreTesting
{
    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddEfCoreInMemory<FooDbContext>()
            .AddDatabaseCleaner<InMemoryDatabaseCleaner>()

            // being tested
            .AddInactivable();
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