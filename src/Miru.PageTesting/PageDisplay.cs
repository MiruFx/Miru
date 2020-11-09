using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Shouldly;

namespace Miru.PageTesting
{
    public class PageDisplay<TModel> : PageElement<TModel>
    {
        public PageDisplay(MiruNavigator nav) : base(nav)
        {
        }
    }
}