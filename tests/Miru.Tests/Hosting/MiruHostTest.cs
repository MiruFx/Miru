using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Hosting;
using Miru.Settings;

namespace Miru.Tests.Hosting;

[TestFixture]
public class MiruHostTest
{
    public class HostTypes
    {
        [Test]
        public async Task Should_run_web_host()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruWebHost<Startup>()
                .ConfigureWebHostDefaults(x => x.UseKestrelAnyLocalPort());

            // act
            var host = await hostBuilder.RunMiruAsync();

            // assert
            host.ShouldBeOfType<WebMiruHost>();
        }
        
        [Test]
        public async Task Should_run_cli_host_when_invoking_consolable()
        {
            // arrange
            var hostBuilder = MiruHost
                .CreateMiruHost("miru", "test.assert")
                .ConfigureServices(x => x.AddConsolable<TestAssertConsolable>());

            // act
            var host = await hostBuilder.RunMiruAsync();
            
            // assert
            host.ShouldBeOfType<CliMiruHost>();
        }
        
        [Test]
        public void Should_run_test_host_when_invoking_test()
        {
            // arrange
            var testConfig = new TestConfig();
            var app = new TestMiruHost().Start(testConfig);
            
            // act
            new SomeFeatureTest().Should_return_miru_app();
            
            // assert
            app.Get<IMiruHost>().ShouldBeOfType<TestMiruHost>();
        }
    }
    
    public class Environment
    {
        [Test]
        public void Run_host_without_environment_should_be_development()
        {
            var sp = MiruHost.CreateMiruHost()
                .Build()
                .Services;
            
            sp.Get<IHostEnvironment>().EnvironmentName.ShouldBe("Development");
        }
        
        [Test]
        public void Run_host_specifying_environment()
        {
            var sp = MiruHost.CreateMiruHost("--environment", "Staging")
                .Build()
                .Services;
            
            sp.Get<IHostEnvironment>().EnvironmentName.ShouldBe("Staging");
        }
        
        [Test]
        public void Run_host_specifying_environment_shortcut()
        {
            var sp = MiruHost.CreateMiruHost("-e", "Test")
                .Build()
                .Services;
            
            sp.Get<IHostEnvironment>().EnvironmentName.ShouldBe("Test");
        }
        
        [Test]
        public void Run_host_with_configuration_from_command_line_args()
        {
            var sp = MiruHost
                .CreateMiruHost("--Database:ConnectionString=DataSource={{ db_dir }}App_dev.db")
                .Build()
                .Services;
            
            sp.Get<DatabaseOptions>().ConnectionString.ShouldBe("DataSource={{ db_dir }}App_dev.db");
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

    public class SomeFeatureTest : FeatureTest
    {
        public void Should_return_miru_app()
        {
            _.Get<IMiruApp>().ShouldBeOfType<MiruApp>();
        }
    }

    public class TestConfig : ITestConfig
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
        }

        public void ConfigureRun(TestRunConfig run)
        {
        }

        public IHostBuilder GetHostBuilder() =>
            MiruHost.CreateMiruWebHost<Startup>();
    }
}