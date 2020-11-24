using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Consolables;
using Miru.Foundation.Hosting;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Hosting
{
    public class HostBuilderExtensionsTest
    {
        [Test]
        public async Task Should_run_miru_cli_host()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruHost("miru", "test:assert")
                .ConfigureServices(services =>
                {
                    services
                        .AddConsolable<TestAssertConsolable>()
                        .AddConsolables<HostBuilderExtensionsTest>()
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
                .ConfigureWebHostDefaults(m => m.UseStartup<Startup>());

            // act
            await hostBuilder.RunMiruAsync();
            
            Startup.Executed.ShouldBeTrue();
        }

        [Oakton.Description("Assert that task ran", Name = "test:assert")]
        public class TestAssertConsolable : ConsolableSync<TestAssertConsolable.Input>
        {
            public static bool Executed { get; private set; }
            
            public class Input { }

            public override bool Execute(Input input) => Executed = true;
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
}