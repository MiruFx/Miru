using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.Testing;
using OpenQA.Selenium.Chrome;
using Playground.Domain;
using Playground.Tests.Config;

namespace Playground.PageTests.Config
{
    public class PageTestsConfig : ITestConfig
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
            // import services from Playground.Tests
            services.AddFrom<TestsConfig>();
            
            services.AddPageTesting<User>(options =>
            {
                if (OS.IsWindows)
                    options.UseChrome(new ChromeOptions().Incognito());
                else
                    options.UseChrome(new ChromeOptions().Incognito().Headless());
            });
        }

        public void ConfigureRun(TestRunConfig run)
        {
            run.PageTestingDefault();
        }
        
        public IHostBuilder GetHostBuilder() => 
            TestMiruHost.CreateMiruHost<Startup>();
    }
}
