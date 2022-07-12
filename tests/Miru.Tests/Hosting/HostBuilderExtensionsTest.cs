using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;

namespace Miru.Tests.Hosting;

public class HostBuilderExtensionsTest
{
    [Test]
    public async Task Should_run_miru_cli_host()
    {
        // arrange
        var hostBuilder = MiruHost
            .CreateMiruHost("miru", "test.assert")
            .ConfigureServices(services =>
            {
                services
                    .AddConsolable<TestAssertConsolable>()
                    .AddSingleton(services);
            });

        // act
        await hostBuilder.RunMiruAsync();
            
        // assert
        TestAssertConsolable.Executed.ShouldBeTrue();
    }
        
    [Test]
    public async Task Should_run_web_host()
    {
        // arrange
        var hostBuilder = MiruHost
            .CreateMiruHost()
            .ConfigureWebHostDefaults(m => m.UseStartup<Startup>().UseKestrelAnyLocalPort());

        // act
        await hostBuilder.RunMiruAsync();
            
        Startup.Executed.ShouldBeTrue();
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