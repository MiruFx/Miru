using System.Collections.Generic;
using System.Threading;
using FluentMigrator.Runner;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Domain;
using Miru.Hosting;

namespace Miru.Tests.Hosting;

[TestFixture]
public class AppInitializerTest
{
    public class Execution
    {
        [Test]
        public async Task Should_execute_initializers_in_parallel()
        {
            // arrange
            var sp = new ServiceCollection()
                .AddHostInitialized<Initializer1>()
                .AddHostInitialized<Initializer2>()
                .BuildServiceProvider();
            
            var initializersRunner = sp.Get<HostInitializedRunner>();

            var stopWatch = new StopWatch();
            stopWatch.Start();

            // act
            await initializersRunner.RunAsync();
            Thread.Sleep(500);

            // assert
            var duration = stopWatch.ElapsedTime();
            
            duration.Milliseconds.ShouldBeLessThan(1000);
            Initializer1.Executed.ShouldBeTrue();
            Initializer2.Executed.ShouldBeTrue();
        }
    }

    public class Hosting
    {
        [Test]
        public async Task Should_run_initializers_after_web_host_start()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruHost()
                .ConfigureWebHostDefaults(m => m.UseStartup<Startup>().UseKestrelAnyLocalPort())
                .ConfigureServices(x =>
                {
                    x.AddHostInitialized<Initializer1>();
                    x.AddHostInitialized<Initializer2>();
                });

            // act
            await hostBuilder.RunMiruAsync();
            Thread.Sleep(500);
            
            Startup.Executed.ShouldBeTrue();
            Initializer1.Executed.ShouldBeTrue();
            Initializer2.Executed.ShouldBeTrue();
        }
        
        [Test]
        public async Task Should_run_initializers_when_invoking_consolable()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruHost("miru", "test.assert")
                .ConfigureServices(services =>
                {
                    services
                        .AddHostInitialized<Initializer1>()
                        .AddHostInitialized<Initializer2>()
                        .AddConsolable<TestAssertConsolable>()
                        .AddSingleton(services);
                });

            // act
            await hostBuilder.RunMiruAsync();
            Thread.Sleep(500);
            
            // assert
            TestAssertConsolable.Executed.ShouldBeTrue();
            Initializer1.Executed.ShouldBeTrue();
            Initializer2.Executed.ShouldBeTrue();
        }
        
        [Test]
        public async Task Should_run_initializers_when_invoking_test()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruHost("miru", "test.assert")
                .ConfigureServices(services =>
                {
                    services
                        .AddHostInitialized<Initializer1>()
                        .AddHostInitialized<Initializer2>()
                        .AddConsolable<TestAssertConsolable>()
                        .AddSingleton(services);
                });

            // act
            await hostBuilder.RunMiruAsync();
            Thread.Sleep(500);
            
            // assert
            TestAssertConsolable.Executed.ShouldBeTrue();
            Initializer1.Executed.ShouldBeTrue();
            Initializer2.Executed.ShouldBeTrue();
        }
        
        [Test]
        public async Task Should_run_initializers_with_scoped_dependencies()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruHost("miru", "test.assert")
                .ConfigureServices(services =>
                {
                    services
                        .AddHostInitialized<InitializerWithScopedDependency>()
                        .AddEfCoreInMemory<FooDbContext>()
                        .AddConsolable<TestAssertConsolable>()
                        .AddSingleton(services);
                });

            // act
            await hostBuilder.RunMiruAsync();
            
            // assert
            Execute
                .Until(() => InitializerWithScopedDependency.Executed, 5.Seconds())
                .ShouldBeTrue();
        }
    }
    
    public class Initializer1 : IHostInitialized
    {
        public static bool Executed;
            
        public async Task InitializeAsync()
        {
            Thread.Sleep(200);
            Executed = true;
            await Task.CompletedTask;
        }
    }
        
    public class Initializer2 : IHostInitialized
    {
        public static bool Executed;
            
        public async Task InitializeAsync()
        {
            Thread.Sleep(200);
            Executed = true;
            await Task.CompletedTask;
        }
    }

    public class InitializerWithScopedDependency : IHostInitialized
    {
        public static bool Executed;
        
        // private readonly FooDbContext _db;

        // public InitializerWithScopedDependency(FooDbContext db)
        // {
        //     _db = db;
        // }

        public async Task InitializeAsync()
        {
            // if (_db is { })
            //     await _db.Posts.FirstOrDefaultAsync();
            
            Executed = true;
            await Task.CompletedTask;
        }
    }
    
    public class Post : Entity
    {
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
    
    public class TestAssertConsolable : Consolable
    {
        public TestAssertConsolable() : base("test.assert")
        {
        }
            
        public static bool Executed { get; private set; }
            
        public class ConsolableHandler : IConsolableHandler
        {
            public Task Execute()
            {
                Executed = true;
                return Task.CompletedTask;
            }
        }
    }
        
    public class Startup
    {
        public static bool Executed { get; private set; }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            Executed = true;

            lifetime.ApplicationStarted.Register(lifetime.StopApplication);
        } 
    }
}