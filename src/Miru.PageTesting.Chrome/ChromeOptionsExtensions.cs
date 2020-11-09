using OpenQA.Selenium.Chrome;

namespace Miru.PageTesting.Chrome
{
    public static class ChromeOptionsExtensions
    {
        public static ChromeOptions Headless(this ChromeOptions options)
        {
            options.AddArguments("--headless");
            return options;
        }
        
        public static ChromeOptions NoSandbox(this ChromeOptions options)
        {
            options.AddArguments("--no-sandbox");
            return options;
        }
        
        public static ChromeOptions DisableGpu(this ChromeOptions options)
        {
            options.AddArguments("--disable-gpu");
            return options;
        }
        
        public static ChromeOptions Incognito(this ChromeOptions options)
        {
            options.AddArgument("--incognito");
            return options;
        }
        
        public static ChromeOptions DisableCache(this ChromeOptions options)
        {
            options.AddArgument("--disable-cache");
            return options;
        }
    }
}