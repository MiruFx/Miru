using NUnit.Framework;
using Shouldly;

namespace Miru.PageTesting.Tests
{
    public class FormTest
    {
        private readonly DriverFixture _ = DriverFixture.Get.Value.ForFirefox();

        [Test]
        public void Can_find_fill_and_submit_form()
        {
            _.HtmlIs(@"
<h1>Login</h1>
<form id=""login-form"" action=""/login"" method=""post"">
    <input type=""text"" name=""Email"" />
    <input type=""submit"" value=""Login"" />
</form>");

            _.Page.Form<Login>(f =>
            {
                f.Input(m => m.Email, "paul@beatles.com");
                
                f.Input(m => m.Email).ShouldBe("paul@beatles.com");
                
                f.Submit();
            });
            
            _.Page.Url.ShouldEndWith("/login");
        }
    }

    public class Login
    {
        public string Email { get; set; }
    }
}