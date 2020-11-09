using System;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Shouldly;

namespace Miru.PageTesting
{
    public class PageElement
    {
        public MiruNavigator Nav { get; }
        
        public PageElement(MiruNavigator nav)
        {
            Nav = nav;
        }

        public void ClickLink(string text)
        {
            var by = By.LinkText(text);
            
            Nav.Logger.LogDebug($"Clicking {by}");
            
            Nav.FindOne(by).Click();
        }
        
        public void Click(By by)
        {
            Nav.Logger.LogDebug($"Clicking {by}");
            
            Nav.FindOne(by).Click();
        }

        public void ShouldHaveTitle(string title)
        {
            Nav.FindAny(ByEx.Title(title), $@"Unable to find the title ""{title}""");
        }

        public void ShouldHaveText(string text)
        {
            Nav.Expect(el => el.Text.Contains(text), $@"Unable to find the text ""{text}""");
        }
        
        public void ShouldHave(By by)
        {
            Nav.Expect(el => Nav.FindAny(by).Displayed, $@"Unable to find ""{by}""");
        }

        public void ShouldHave(By by, object expected)
        {
            Nav.Logger.LogDebug($@"Searching display label {by} with ""{expected}""");
            
            var text = Nav.FindOne(by).Text();
            
            // TODO: show message: Display for '{elementName}' should be {expected} but was {actual}'
            text.ShouldContain(expected.ToString());
        }
        
        public void ShouldNotHaveText(string text)
        {
            Nav.NotExpect(el => el.Text.Contains(text), $@"Unable to find the text ""{text}""");
        }

        public void Form<TModel>(Action<PageElement<TModel>> action)
        {
            var formId = Nav.Naming.Form(typeof(TModel));

            var by = By.Id(formId);
            
            Nav.Logger.LogDebug($"Form {by}");
            
            Within(by, action);
        }
        
        public void Display<TModel>(Action<PageDisplay<TModel>> action)
        {
            var displayId = Nav.Naming.Display(typeof(TModel));

            var by = By.Id(displayId);
            
            Nav.Logger.LogDebug($"Display {by}");
            
            action(new PageDisplay<TModel>(Nav.FindOne(by)));
        }
        
        public void Submit()
        {
            Nav.Logger.LogDebug("Submitting form");
            
            Nav.FindOne(ByEx.Submit()).Click();
        }

        public void Within(By by, Action<PageElement> action)
        {
            Nav.Logger.LogDebug($"Scoping {by}");
            
            var pageElement = new PageElement(Nav.FindOne(by));
            
            action(pageElement);
        }
        
        public void Within<TModel>(By by, Action<PageElement<TModel>> action)
        {
            Nav.Logger.LogDebug($"Scoping {by}");
            
            var pageElement = new PageElement<TModel>(Nav.FindOne(by));
            
            action(pageElement);
        }
    }
}