using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miru.Html;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Miru.PageTesting.Chrome;

public static class PageTestingConfigExtensions
{
    public static void UseChrome(this PageTestingConfig config, ChromeOptions options = null)
    {
        config.Services.AddSingleton<WebDriver>(ctx =>
        {
            return new ChromeDriver(options ?? new ChromeOptions());
        });
            
        config.Services.AddSingleton<MiruNavigator>(sp =>
        {
            var driver = sp.GetService<WebDriver>();

            return new ChromeNavigator(
                sp.GetService<WebDriverWait>(),
                sp.GetService<ElementNaming>(),
                () => driver.FindElement(By.TagName("body")),
                driver,
                sp.GetService<ILogger<MiruNavigator>>());
        });
    }
}