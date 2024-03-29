using System;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Miru.PageTesting;

public static class PageElementExtensions
{
    public static void FormFor<TModel>(this PageElement page, Action<PageElement<TModel>> action)
    {
        var formId = page.Nav.Naming.Form(typeof(TModel));

        var by = By.Id(formId);
            
        page.Nav.Logger.LogDebug($"Form {by}");
        
        page.Within(by, action);
    }
        
    public static void Refresh(this PageElement page)
    {
        page.Nav.Driver.Navigate().Refresh();
    }
}