using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Miru.PageTesting
{
    public static class WebElementExtensions
    {
        public static string ToHtml(this IWebElement webElement, RemoteWebDriver driver)
        {
            return (string) driver.ExecuteScript("return arguments[0].innerHTML;", webElement);
        }
    }
}