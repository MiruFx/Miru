using System;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Remote;

namespace Miru.PageTesting.Firefox
{
    public static class ServiceSeleniumExtensions
    {
        public static void AddSeleniumFirefox(this IServiceCollection services, Func<RemoteWebDriver> driverFactory)
        {
            services.AddSingleton(driverFactory());
        }
        
        public static IServiceCollection AddSelenium(this IServiceCollection services, Func<IServiceProvider, RemoteWebDriver> driverFactory)
        {
            return services.AddSingleton(driverFactory);
        }
    }
}