using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miru.Html;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Miru.PageTesting.Firefox;

public static class PageTestingConfigExtensions
{
    public static void UseFirefox(this PageTestingConfig config, FirefoxOptions options = null)
    {
        config.Services.AddSelenium(ctx => new FirefoxDriver(options ?? new FirefoxOptions()));
            
        config.Services.AddSingleton<MiruNavigator>(sp =>
        {
            var driver = sp.GetService<WebDriver>();

            return new FirefoxNavigator(
                sp.GetService<WebDriverWait>(),
                sp.GetService<ElementNaming>(),
                () => driver.FindElement(By.TagName("body")),
                driver,
                sp.GetService<ILogger<MiruNavigator>>());
        });
    }
}