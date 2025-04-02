using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.UserStamp;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Tests.Databases.EntityFramework;
using Miru.Userfy;

namespace Miru.Tests.Behaviors;

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
            .AddMiruCoreTesting()
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
        _.Logout();
    }

    [Test]
    public void Should_set_current_user_for_new_entity()
    {
        // arrange
        var user = _.Make<User>();
        _.Save(user);
        _.LoginAs(user);
            
        var post = new Post {Title = "Hello"};
            
        _.LoginAs(user);
            
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
        var otherUser = _.Make<User>();
        var currentUser = _.Make<User>();
            
        _.Save(otherUser, currentUser);
            
        var post = _.Make<Post>(x =>
        {
            x.CreatedById = otherUser.Id;
            x.UpdatedById = otherUser.Id;
        });
            
        _.Save(post);
            
        _.LoginAs(currentUser);
            
        // act
        _.Save(post);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Posts.First());
        saved.CreatedById.ShouldBe(otherUser.Id);
        saved.UpdatedById.ShouldBe(currentUser.Id);
    }
        
    [Test]
    public void When_nullable_and_if_no_logged_user_then_should_not_stamp()
    {
        // arrange
        var category = new Category {Name = "Politics"};
            
        // act
        _.Save(category);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Categories.First());
        saved.CreatedById.ShouldBeNull();
        saved.UpdatedById.ShouldBeNull();
    }
        
    [Test]
    public void When_nullable_and_has_logged_user_then_should_stamp()
    {
        // arrange
        var category = new Category { Name = "Politics" };
        var currentUser = _.Make<User>();

        _.Save(currentUser);
        _.LoginAs(currentUser);
            
        // act
        _.Save(category);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Categories.First());
        saved.CreatedById.ShouldBe(currentUser.Id);
        saved.UpdatedById.ShouldBe(currentUser.Id);
    }

    [Test]
    public void If_nullable_and_no_logged_user_and_entity_was_saved_stamped_then_should_update_only_updated_by_id()
    {
        // arrange
        var otherUser = _.Make<User>();
        _.Save(otherUser);
            
        var category = _.Make<Category>(x =>
        {
            x.CreatedById = otherUser.Id;
            x.UpdatedById = otherUser.Id;
        });
        _.Save(category);
            
        // act
        _.Save(category);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Categories.First());
        saved.CreatedById.ShouldBe(otherUser.Id);
        saved.UpdatedById.ShouldBeNull();
    }
        
    [Test]
    public void If_nullable_and_has_logged_user_and_entity_was_saved_stamped_then_should_update_only_updated_by_id()
    {
        // arrange
        var otherUser = _.Make<User>();
        var currentUser = _.Make<User>();
            
        _.Save(otherUser, currentUser);
            
        var category = _.Make<Category>(x =>
        {
            x.CreatedById = otherUser.Id;
            x.UpdatedById = otherUser.Id;
        });
            
        _.Save(category);
        _.LoginAs(currentUser);
            
        // act
        _.Save(category);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Categories.First());
        saved.CreatedById.ShouldBe(otherUser.Id);
        saved.UpdatedById.ShouldBe(currentUser.Id);
    }
        
    public class Post : Entity, IUserStamped
    {
        public string Title { get; set; }
            
        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
    }
        
    public class Category : Entity, ICanBeUserStamped
    {
        public string Name { get; set; }
            
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }

    public class User : UserfyUser
    {
        public override string Display => UserName;
    }
        
    public class FooDbContext(
        DbContextOptions options,
        IEnumerable<IInterceptor> interceptors) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
            
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(interceptors);
        }
    }
}