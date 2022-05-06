using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using Miru.Html;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Miru.PageTesting;

public abstract class MiruNavigator : IDisposable
{
    public readonly WebDriverWait Wait;
    public readonly ElementNaming Naming;
    public readonly WebDriver Driver;
    public Func<PageExceptionContext, Exception> ExceptionHandle;
    public Func<IWebElement> Element { get; }
    public ILogger<MiruNavigator> Logger { get; }

    private static readonly Func<PageExceptionContext, Exception> DefaultExceptionFunc = context =>
    {
        var exceptionMessage = context.FailureMessage.Or(context.OriginalException.Message);

        throw new PageTestException(exceptionMessage, context.OriginalException);
    };
        
    public MiruNavigator(
        WebDriverWait wait,
        ElementNaming naming,
        Func<IWebElement> element,
        WebDriver driver,
        ILogger<MiruNavigator> logger,
        Func<PageExceptionContext, Exception> exceptionFunc = null)
    {
        Wait = wait;
        Naming = naming;
        Driver = driver;
        Element = element;
        Logger = logger;
            
        ExceptionHandle = exceptionFunc ?? DefaultExceptionFunc;
    }

    public MiruNavigator FindOne(By by)
    {
        return Handle(() =>
        {
            try
            {
                var element = Wait.Until(_ => 
                    Element().FindElements(by).SingleOrDefault(m => m.Displayed));

                // var webElement = elements.Count switch
                // {
                //     1 => elements.First(),
                //
                //     0 => throw new ElementNotFoundException($"Unable to find one element '{by}'"),
                //
                //     _ => throw new ManyElementsFoundException($"{elements.Count} elements found for '{by}' when was expecting one")
                // };

                return CreateSubNavigator(element);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Sequence contains more than one matching element"))
                    throw new ManyElementsFoundException($"Many elements found for '{by}' when was expecting one");

                throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new ElementNotFoundException($"Unable to find one element '{by}'", ex);
            }
        });
    }

    public void ConfigureExceptions(Func<PageExceptionContext, Exception> action)
    {
        ExceptionHandle = action;
    }

    public abstract MiruNavigator CreateSubNavigator(IWebElement webElement);

    public virtual void Click()
    {
        Handle(() => Element().Click());
    }

    public string Text()
    {
        return Handle(() => Element().Text);
    }

    public string GetAttribute(string name)
    {
        return Element().GetAttribute(name);
    }

    private Exception TransformException(string failureMessage, Exception ex)
    {
        var context = new PageExceptionContext(this, ex, failureMessage);

        return ExceptionHandle?.Invoke(context);
    }

    public void Expect(Func<bool> func, string failureMessage)
    {
        Handle(() => Wait.Until(drv => func()), failureMessage);
    }
        
    public void Expect(Func<IWebElement, bool> func, string failureMessage)
    {
        Handle(() => Wait.Until(drv => func(Element())), failureMessage);
    }

    public void NotExpect(Func<IWebElement, bool> func, string failureMessage)
    {
        Handle(() => Wait.Until(drv => func(Element()) == false), failureMessage);
    }

    public virtual void CloseDrivers()
    {
        Driver.Quit();
        Driver.Dispose();
    }
        
    public void ScrollToCurrentElement()
    {
        ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", Element());
    }

    public void ExecuteScript(string script, params object[] args)
    {
        Driver.ExecuteScript(script, args);
    }
        
    public ReadOnlyCollection<IWebElement> FindMany(By by, string failureMessage = null)
    {
        try
        {
            return Wait.Until(drv => Element().FindElements(by));
        }
        catch (WebDriverTimeoutException ex)
        {
            throw new ElementNotFoundException(failureMessage.Or($"Unable to find any element {by}"), ex);
        }
    }
        
    public IWebElement FindAny(By by, string failureMessage = null)
    {
        try
        {
            var attempts = 1;

            while(attempts <= 3)
            {
                try
                {
                    return Wait.Until(drv => Element().FindElement(by));
                } 
                catch(StaleElementReferenceException)
                {
                    attempts++;
                }
            } 
        }
        catch (WebDriverTimeoutException ex)
        {
            throw new ElementNotFoundException(failureMessage.Or($"Unable to find any element {by}"), ex);
        }
            
        throw new ElementNotFoundException(failureMessage.Or($"Unable to find any element {by}"));
    }
        
    public void Input(By by, string value)
    {
        var input = FindOne(by).Element();

        if (input.GetAttribute("type").Equals("text") || input.GetAttribute("type").Equals("password"))
        {
            Logger.LogDebug($@"Input {by}: ""{value}""");

            input.Clear();
            input.SendKeys(value);
        }
        if (input.TagName.Equals("textarea"))
        {
            Logger.LogDebug($@"TextArea {by}: ""{value}""");

            input.Clear();
            input.SendKeys(value);
        }
        else if (input.GetAttribute("type").Equals("select-one"))
        {
            Logger.LogDebug($@"Select {by}: ""{input}""");
                
            var selectElement = new SelectElement(input);
            selectElement.SelectByText(value);
        }
            
        // TODO: Make a InputRadio
        // else if (input.GetAttribute("type").Equals("radio"))
        // {
        //     var option = input.FirstOrDefault(m => m.GetAttribute("value").Equals(value));
        //     
        //     if (option == null)
        //         throw new PageTestException(
        //             $"Could not find an input[type=radio] with value '{value}'");
        //     
        //     option.Click();
        // }
            
        // var inputs = FindMany(by);
        //
        // if (inputs.Count == 1 && inputs[0].GetAttribute("type").Equals("text") || inputs[0].GetAttribute("type").Equals("password"))
        // {
        //     Logger.LogDebug($@"Input {by}: ""{value}""");
        //
        //     inputs[0].Clear();
        //     inputs[0].SendKeys(value);
        // }
        // if (inputs.Count == 1 && inputs[0].TagName.Equals("textarea"))
        // {
        //     Logger.LogDebug($@"TextArea {by}: ""{value}""");
        //
        //     inputs[0].Clear();
        //     inputs[0].SendKeys(value);
        // }
        // else if (inputs.Count == 1 && inputs[0].GetAttribute("type").Equals("select-one"))
        // {
        //     Logger.LogDebug($@"Select {by}: ""{inputs[0]}""");
        //     
        //     var selectElement = new SelectElement(inputs[0]);
        //     selectElement.SelectByText(value);
        // }
        // else if (inputs.Count >= 1 && inputs[0].GetAttribute("type").Equals("radio"))
        // {
        //     var option = inputs.FirstOrDefault(m => m.GetAttribute("value").Equals(value));
        //     
        //     if (option == null)
        //         throw new PageTestException(
        //             $"Could not find an input[type=radio] with value '{value}'");
        //     
        //     option.Click();
        // }
        //
        // TODO: Exception
    }

    public virtual void Dispose()
    {
        Driver.Dispose();
    }
        
    protected TReturn Handle<TReturn>(Func<TReturn> action, string failureMessage = null)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            throw TransformException(failureMessage, ex);
        }
    }
        
    protected void Handle(Action action, string failureMessage = null)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            throw TransformException(failureMessage, ex);
        }
    }
}