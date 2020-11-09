using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Miru;
using Miru.PageTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SelfImprov.PageTests
{
    public static class MiruExtensions
    {
        public static void Check<TModel>(
            this PageElement<TModel> page, 
            Expression<Func<TModel, bool>> property)
        {
            var inputId = page.Nav.Naming.Input(property);

            var by = By.Name(inputId);
            
            var input = page.Nav.FindOne(by).Element();

            if (input.GetAttribute("type").Equals("checkbox"))
            {
                input.Click();
                page.Nav.Logger.LogDebug($@"Check {by}");
            }
        }
        
        public static void FormFor<TModel>(this PageElement page, string id, Action<PageElement<TModel>> action)
        {
            var formId = page.Nav.Naming.Id(typeof(TModel));

            var by = id.NotEmpty() ? By.Id(id) : By.CssSelector($"[data-for='{formId}']");
            
            page.Nav.Logger.LogDebug($"Form {by}");
            
            page.Within(by, action);
        }
        
        public static void Input2<TModel, TProperty>(
            this PageElement<TModel> page, 
            Expression<Func<TModel, TProperty>> property, 
            string value)
        {
            var inputId = page.Nav.Naming.Input(property);
            
            var input = page.Nav.FindOne(By.Id(inputId)).Element();

            if (input.GetAttribute("type").Equals("text") || input.GetAttribute("type").Equals("password"))
            {
                // Logger.LogDebug($@"Input {by}: ""{value}""");

                input.Clear();
                input.SendKeys(value);
            }
            if (input.TagName.Equals("textarea"))
            {
                // Logger.LogDebug($@"TextArea {by}: ""{value}""");

                input.Clear();
                input.SendKeys(value);
            }
            else if (input.GetAttribute("type").Equals("select-one"))
            {
                // Logger.LogDebug($@"Select {by}: ""{input}""");
                
                var selectElement = new SelectElement(input);
                selectElement.SelectByText(value);
            }
        }
        
        public static void ShouldNotHaveText2(
            this PageElement page,
            string text)
        {
            page.Nav.NotExpect(el => el.Text.Contains(text), $@"Was not expecting but it was found the text ""{text}""");
        }
    }
}