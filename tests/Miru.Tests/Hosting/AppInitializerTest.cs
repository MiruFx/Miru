using System;
using System.Threading;
using FluentMigrator.Runner;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Hosting;

namespace Miru.Tests.Hosting;

public class AppInitializerTest
{
    public class Execution
    {
        [Test]
        public async Task Should_execute_initializers_in_parallel()
        {
            // arrange
            var sp = new ServiceCollection()
                .AddInitializer<Initializer1>()
                .AddInitializer<Initializer2>()
                .BuildServiceProvider();
            
            var initializersRunner = sp.Get<AppInitializerRunner>();

            var stopWatch = new StopWatch();
            stopWatch.Start();

            // act
            await initializersRunner.RunAsync();

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
                    x.AddInitializer<Initializer1>();
                    x.AddInitializer<Initializer2>();
                });

            // act
            await hostBuilder.RunMiruAsync();
            
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
                        .AddInitializer<Initializer1>()
                        .AddInitializer<Initializer2>()
                        .AddConsolable<TestAssertConsolable>()
                        .AddSingleton(services);
                });

            // act
            await hostBuilder.RunMiruAsync();
            
            // assert
            TestAssertConsolable.Executed.ShouldBeTrue();
            Initializer1.Executed.ShouldBeTrue();
            Initializer2.Executed.ShouldBeTrue();
        }
    }
    
    public class Initializer1 : IAppInitializer
    {
        public static bool Executed;
            
        public async Task InitializeAsync()
        {
            Thread.Sleep(1.Seconds());
            Executed = true;
            await Task.CompletedTask;
        }
    }
        
    public class Initializer2 : IAppInitializer
    {
        public static bool Executed;
            
        public async Task InitializeAsync()
        {
            Thread.Sleep(1.Seconds());
            Executed = true;
            await Task.CompletedTask;
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