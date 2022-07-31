using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Firefox;
using Miru.Testing;
using OpenQA.Selenium.Firefox;
using Playground.Domain;
using Playground.Tests.Config;

namespace Playground.PageTests.Config;

public class PageTestsConfig : ITestConfig
{
    public void ConfigureTestServices(IServiceCollection services)
    {
        // import services from Playground.Tests
        services.AddFrom<TestsConfig>();

        services.AddPageTesting<User>();
            
        services.AddPageTesting<User>(options =>
        {
            if (OS.IsWindows)
                options.UseFirefox();
            else
                options.UseFirefox(new FirefoxOptions().Headless());
        });
    }

    public void ConfigureRun(TestRunConfig run)
    {
        run.PageTestingDefault();
    }
        
    public IHostBuilder GetHostBuilder() => 
        TestMiruHost.CreateWebMiruHost<Startup>();
}