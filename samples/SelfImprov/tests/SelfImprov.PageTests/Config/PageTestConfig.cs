using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.Storages;
using Miru.Testing;
using Miru.Testing.Userfy;
using OpenQA.Selenium.Chrome;
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
            
            services.AddPageTesting(options =>
            {
                options.TimeOut = 3.Seconds();
                
                if (OS.IsWindows)
                    options.UseChrome(new ChromeOptions().Incognito());
                else
                    options.UseChrome(new ChromeOptions().Incognito().Headless());
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
