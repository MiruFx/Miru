using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Foundation.Logging;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.PageTesting.Firefox;
using Miru.Testing;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Payments;
using Mong.Tests.Config;
using NSubstitute;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Serilog.Events;

namespace Mong.PageTests.Config
{
    public class PageTestsConfig : ITestConfig
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
            services.AddFrom<TestsConfig>();
            
            services.AddPageTesting(options =>
            {
                if (OS.IsWindows)
                    options.UseFirefox(new FirefoxOptions());
                else
                    options.UseFirefox(new FirefoxOptions().Headless()); 
            });

            // --miru-log debug | --aspnet-log debug --efcoresql-log debug
            services.AddSerilogConfig(cfg =>
            {
                cfg.PageTesting(LogEventLevel.Debug);
            });
        }

        public void ConfigureRun(TestRunConfig run)
        {
            run.PageTestingDefault();

            run.UserfyRequiresAdmin<User>();
            
            run.BeforeSuite(_ =>
            {
                // configure some default returns for mocks
                _.Get<IPayPau>()
                    .Charge(default, default)
                    .ReturnsForAnyArgs(new PayPauResult
                    {
                        TransactionId = Guid.NewGuid().ToString()
                    });
            });
        }
        
        public IHostBuilder GetHostBuilder() => 
            TestMiruHost.CreateMiruHost<Startup>();
    }
}