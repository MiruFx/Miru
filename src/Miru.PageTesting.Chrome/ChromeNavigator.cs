using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Miru.Html;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Miru.PageTesting.Chrome;

public class ChromeNavigator : MiruNavigator
{
    public ChromeNavigator(
        WebDriverWait wait, 
        ElementNaming naming, 
        Func<IWebElement> element,
        WebDriver driver, 
        ILogger<MiruNavigator> logger,
        Func<PageExceptionContext, Exception> exceptionFunc = null) : base(wait, naming, element, driver, logger, exceptionFunc)
    {
    }

    public override MiruNavigator CreateSubNavigator(IWebElement webElement)
    {
        return new ChromeNavigator(Wait, Naming, () => webElement, Driver, Logger, ExceptionHandle);
    }
        
    public override void CloseDrivers()
    {
        base.CloseDrivers();
          
        Logger.LogDebug("Killing all 'chromedriver' processes");
            
        Kill.Processes("chromedriver");
    }
}