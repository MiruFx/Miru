using System;
using System.Collections.Generic;
using System.Threading;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Pipeline;
using Miru.Sqlite;
using Miru.Tests.Databases.EntityFramework;
using Miru.Tests.Scheduling;

namespace Miru.Tests.Behaviors;

public class DomainEventsTest : MiruCoreTest
{
    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddEfCoreSqlite<FooDbContext>(connectionString: "DataSource={{ db_dir }}DomainEventTest.db")
            .AddDatabaseCleaner<SqliteDatabaseCleaner>()
            .AddDefaultPipeline<DomainEventsTest>()
            .AddInMemoryQueueing()
            .AddTransient(sp => new BackgroundJobServer(sp.GetService<JobStorage>()))

            // being tested
            .AddDomainEvents();
    }

    [OneTimeSetUp]
    public void SetupOnce()
    {
        var server = _.Get<BackgroundJobServer>();
        
        using var scope = _.WithScope();
        var db = scope.Get<FooDbContext>();
        db.Database.EnsureCreated();
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
    
    [Test]
    public async Task Should_process_enqueued_event()
    {
        // arrange
        var post = new Post("Original post");
            
        // act
        await _.SaveAsync(post);
        
        // assert
        post.EnqueueEvents.ShouldBeEmpty();
        
        Execute.Until(() => PostArchive.Processed, TimeSpan.FromSeconds(2));
        
        _.EnqueuedCount().ShouldBe(0);
    }
}

public class Post : EntityEventable
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
    public bool Archived { get; set; }
}

public class PostCreated : IDomainEvent, IEnqueuedEvent
{
    public Post Post { get; }

    public PostCreated(Post post)
    {
        Post = post;
    }

    public INotification GetNotification()
    {
        return new PostCreatedJob
        {
            PostId = Post.Id
        };
    }
}

public class PostCreatedJob : INotification
{
    public long PostId { get; set; }
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

        await Task.CompletedTask;
        // await _db.SaveChangesAsync(ct);
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

public class PostArchive
{
    public static bool Processed { get; set; }

    public class Handler : INotificationHandler<PostCreatedJob>
    {
        private readonly FooDbContext _db;

        public Handler(FooDbContext db)
        {
            _db = db;
        }

        public async Task Handle(PostCreatedJob notification, CancellationToken ct)
        {
            Processed = true;

            await Task.CompletedTask;
        }
    }
}