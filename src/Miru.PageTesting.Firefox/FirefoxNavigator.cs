using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Miru.Html;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Miru.PageTesting.Firefox
{
    public class FirefoxNavigator : MiruNavigator
    {
        // TODO: Firefox require scroll if input is not visible
        // if (scroll)
        // {
        //     ((IJavaScriptExecutor) _driver).ExecuteScript("arguments[0].scrollIntoView(true);", input);
        //     
        //     // var action = new OpenQA.Selenium.Interactions.Actions(_driver);
        //     // action.MoveToElement(input);
        //     // action.Perform();
        // }
    
        public FirefoxNavigator(
            WebDriverWait wait, 
            ElementNaming naming, 
            Func<IWebElement> element,
            RemoteWebDriver driver, 
            ILogger<MiruNavigator> logger,
            Func<PageExceptionContext, Exception> exceptionFunc = null) : base(wait, naming, element, driver, logger, exceptionFunc)
        {
        }

        public override MiruNavigator CreateSubNavigator(IWebElement webElement)
        {
            return new FirefoxNavigator(Wait, Naming, () => webElement, Driver, Logger, ExceptionHandle);
        }
        
        public override void Click()
        {
            // Firefox has two interesting things when clicking to a link
            // - throws exception when link is not visible in the screen 
            // - can't click in a link with a button inside 
            ScrollToCurrentElement();
            
            // find all buttons inside current link
            var buttons = FindMany(By.TagName("button"));
            
            if (buttons.Count == 0)
                base.Click();
            else if (buttons.Count >= 1)
                buttons.First().Click();
        }

        public override void CloseDrivers()
        {
            base.CloseDrivers();
            
            Kill.Processes("geckodriver");
        }
    }
}