using OpenQA.Selenium.Firefox;

namespace Miru.PageTesting.Firefox
{
    public static class FirefoxOptionsExtensions
    {
        public static FirefoxOptions Headless(this FirefoxOptions options)
        {
            options.AddArgument("-headless");

            return options;
        }
    }
}