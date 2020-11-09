using Miru.PageTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using Shouldly;

namespace Miru.PageTesting.Tests
{
    public class WithinTest
    {
        private readonly DriverFixture _ = DriverFixture.Get.Value.ForFirefox();
        
        [Test]
        public void Can_make_expectations_within_element()
        {
            _.HtmlIs(@"
<h1>Title</h1>
<form id=""login-form"">
    <h3>Login here</h3>
    Forgot password?
</form>");

            _.Page.Within(By.Id("login-form"), m =>
            {
                m.ShouldHaveText("Forgot password?");
                
                m.ShouldHaveTitle("Login here");
                
                m.ShouldNotHaveText("Title");
            });
        }
        
        [Test]
        public void Can_use_nested_within()
        {
            _.HtmlIs(@"
<h1>Products</h1>
<div id=""products"">
    <div id=""products-1"">
        iPhone
    </div>
</div>");

            _.Page.Within(By.Id("products"), m =>
            {
                m.Within(By.Id("products-1"), p =>
                {
                    p.ShouldHaveText("iPhone");
                    
                    p.ShouldNotHaveText("Products");
                });
            });
        }
        
        [Test]
        public void Can_interact_within_elements()
        {
            _.HtmlIs(@"
<h1>Products</h1>
<div id=""products"">
    <div id=""products-1"">
        <a href=""/product/1"">View Product</a>
    </div>
</div>");

            _.Page.Within(By.Id("products"), m =>
            {
                m.Within(By.Id("products-1"), p =>
                {
                    p.ClickLink("View Product");
                });
            });
            
            _.Page.Url.ShouldEndWith("/product/1");
        }
    }
}