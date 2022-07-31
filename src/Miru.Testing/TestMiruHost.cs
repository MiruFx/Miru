using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Baseline;
using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Foundation;
using Miru.Hosting;
using Miru.Storages;
using Miru.Userfy;
using Serilog;

namespace Miru.Testing;

public class TestMiruHost : IMiruHost
{
    internal static IMiruApp App { get; set; }

    public static bool UsingTestRunner = false;
        
    private TestRunConfig _testRunConfig;

    public static IMiruApp StartOrGetApp(Assembly assemblyWithTestConfig)
    {
        if (App == null)
        {
            var config = TestConfigFinder.Find(assemblyWithTestConfig);

            new TestMiruHost().Start(config);
        }

        return App;
    }

    public static IHostBuilder CreateWebMiruHost<TStartup>() where TStartup : class
    {
        return MiruHost.CreateMiruWebHost<TStartup>()
            .ConfigureWebHost(host =>
            {
                // we don't want the tests fail because there is no tcp port available
                // so here, listen to any available port
                host.UseKestrelAnyLocalPort();
            });
    }
        
    public static void ExecuteBeforeSuite() => App.Get<TestConfigRunner>().RunBeforeSuite();
        
    public static void ExecuteAfterSuite() => App.Get<TestConfigRunner>().RunAfterSuite();
        
    public IMiruApp Start(ITestConfig config, Action<IServiceCollection> servicesSetup = null)
    {
        _testRunConfig = new TestRunConfig();
            
        var servicesFromTestConfig = new ServiceCollection();
            
        config.ConfigureRun(_testRunConfig);
            
        config.ConfigureTestServices(servicesFromTestConfig);

        var builder = config.GetHostBuilder();

        builder.UseEnvironment("Test");
            
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(this);
                        
            services.AddSingleton<TestFixture, TestFixture>();
                    
            services.AddSingleton<ISessionStore, MemorySessionStore>();
                    
            services.AddSingleton<Faker, Faker>();

            services.AddRange(servicesFromTestConfig);

            // test engine stuff
            services.AddSingleton<TestConfigRunner>();
                
            services.AddSingleton(_testRunConfig);
                
            servicesSetup?.Invoke(services);

            services.AddSingleton<IMiruHost>(sp => this);
        });
            
        var host = builder.Build();

        var sp = host.Services;
            
        App =  sp.GetService<IMiruApp>();

        host.RunAppInitializers().GetAwaiter().GetResult();

        ExecuteBeforeSuite();

        return App;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        await Task.CompletedTask;
    }
}