using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;

namespace Miru.PageTesting
{
    public static class ServiceChromeExtensions
    {
        public static IServiceCollection AddSeleniumChrome(this IServiceCollection services, ChromeOptions options = null)
        {
            return services.AddSelenium(container =>
            {
                if (options == null)
                {
                    options = new ChromeOptions();
                    // .NoSandbox()
                    // .DisableCache()
                    // .Incognito()
                    // .DisableGpu();
                }
                
                return new ChromeDriver(options);
            });
        }
    }
}