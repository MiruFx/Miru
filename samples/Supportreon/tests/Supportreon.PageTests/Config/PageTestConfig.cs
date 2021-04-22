using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.Storages;
using Miru.Testing;
using Miru.Testing.Userfy;
using OpenQA.Selenium.Chrome;
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
                    options.UseChrome();
                else
                    options.UseChrome(new ChromeOptions().Headless());
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
            TestMiruHost.CreateMiruHost<Startup>();
    }
}
