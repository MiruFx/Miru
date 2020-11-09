using System;
using Microsoft.Extensions.Logging;
using Miru.Foundation;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Miru.PageTesting
{
    public class PageBody : PageElement
    {
        public PageBody(MiruNavigator navigator) : base(navigator)
        {
        }

        public string Url => Nav.Driver.Url;
        
        public string BaseUrl { get; set; }

        public void NavigateTo(string path)
        {
            Nav.Logger.LogDebug($"Navigating to {path}");
            
            Nav.Driver.Navigate().GoToUrl(UrlFor(path));
        }
        
        public string UrlFor(string path)
        {
            return $"{BaseUrl}{path}";
        }
    }
}

    