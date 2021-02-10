using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.PageTesting.Firefox;
using Miru.Storages;
using Miru.Testing;
using Miru.Testing.Userfy;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SelfImprov.Domain;
using SelfImprov.Tests.Config;

namespace SelfImprov.PageTests.Config
{
    public class PageTestsConfig : ITestConfig
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
            // import services from SelfImprov.Tests
            services.AddFrom<TestsConfig>();
            
            services.AddPageTesting<User>(options =>
            {
                if (OS.IsWindows)
                    options.UseFirefox(new FirefoxOptions());
                else
                    options.UseFirefox(new FirefoxOptions().Headless());
            });
        }

        public void ConfigureRun(TestRunConfig run)
        {
            run.PageTestingDefault();
            
            run.UserfyRequires<User>();
        }
        
        public IHostBuilder GetHostBuilder() => 
            TestMiruHost.CreateMiruHost<Startup>();
    }
}
