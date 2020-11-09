using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Shouldly;

namespace Miru.PageTesting
{
    public class PageElement<TModel> : PageElement
    {
        public PageElement(MiruNavigator nav) : base(nav)
        {
        }
        
        public void Input<TProperty>(Expression<Func<TModel, TProperty>> property, object value)
        {
            var inputId = Nav.Naming.Input(property);
            
            Nav.Input(By.Name(inputId), value.ToString());
        }
        
        public string Input<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            var inputId = Nav.Naming.Input(property);

            return Nav.FindOne(By.Name(inputId)).GetAttribute("value");
        }

        public void Select(Expression<Func<TModel, object>> property, string value)
        {
            var name = Nav.Naming.Input(property);
            
            Nav.Input(By.Name(name), value);
        }
        
        public void ShouldHave<TProperty>(Expression<Func<TModel, TProperty>> property, object expected)
        {
            var id = Nav.Naming.DisplayLabel(property);

            var by = By.Id(id);

            ShouldHave(by, expected);
        }
    }
}