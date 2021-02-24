using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Firefox;
using Miru.Storages;
using Miru.Testing;
using Miru.Testing.Userfy;
using OpenQA.Selenium.Firefox;
using Supportreon.Domain;
using Supportreon.Tests.Config;

namespace Supportreon.PageTests.Config
{
    public class PageTestsConfig : ITestConfig
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
            // import services from Supportreon.Tests
            services.AddFrom<TestsConfig>();
            
            // FIXME: should not pass User here, should add other services.Replace<IUserSession>
            services.AddPageTesting<User>(options =>
            {
                if (OS.IsWindows)
                    options.UseFirefox(new FirefoxOptions());
                else
                    options.UseFirefox(new FirefoxOptions().Headless());
            });

            services.AddSingleton(sp => TestLoggerConfigurations.ForPageTest(sp.GetService<Storage>()));
        }

        public void ConfigureRun(TestRunConfig run)
        {
            run.PageTestingDefault();
            
            run.BeforeAll<IRequiresAuthenticatedUser>(_ =>
            {
                _.MakeSavingLogin<User>();
            });
        }
        
        public IHostBuilder GetHostBuilder() => 
            // MiruHost.CreateMiruHost<Startup>()
            //     .ConfigureWebHostDefaults(host =>
            //     {
            //         // we don't want the tests fail because there is no tcp port available
            //         // so here, listen to any available port
            //         host.UseKestrelAnyLocalPort();
            //     })
            //     .UseDefaultServiceProvider((context, options) =>
            //     {
            //         options.ValidateScopes = context.HostingEnvironment.IsDevelopmentOrTest();
            //     });
            TestMiruHost.CreateMiruHost<Startup>();
    }
}
