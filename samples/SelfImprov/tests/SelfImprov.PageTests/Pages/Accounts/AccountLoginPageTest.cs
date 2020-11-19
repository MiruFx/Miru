using Miru.PageTesting;
using Miru.Testing;
using SelfImprov.Features.Accounts;
using NUnit.Framework;
using SelfImprov.Domain;

namespace SelfImprov.PageTests.Pages.Accounts
{
    public class AccountLoginPageTest : PageTest
    {
        private User _user;

        [SetUp]
        public void Setup()
        {
            _.Logout();
            
            _.Visit<AccountLogin>();
        }

        public override void Given()
        {
            _user = _.MakeSaving<User>();
        }

        [Test]
        public void Can_login()
        {
            _.Form<AccountLogin.Command>((f, command) =>
            {
                f.Input(m => m.Email, _user.Email);
                f.Input(m => m.Password, "123456");
                f.Submit();
            });
            
            _.ShouldRedirectTo("/");
        }

        [Test]
        public void Should_show_summary_when_request_is_invalid()
        {
            _.Form<AccountLogin.Command>((f, command) =>
            {
                f.Input(m => m.Email, string.Empty);
                f.Input(m => m.Password, string.Empty);
                f.Submit();
            });
            
            _.ShouldHaveText("'Email' must not be empty.");
            _.ShouldHaveText("'Password' must not be empty.");
        }
        
        [Test]
        public void Should_show_summary_for_user_and_password_not_found()
        {
            _.Form<AccountLogin.Command>((f, command) =>
            {
                f.Input(m => m.Email, command.Email);
                f.Input(m => m.Password, command.Password);
                f.Submit();
            });
            
            _.ShouldHaveText("User and password not found");
        }
    }
}
