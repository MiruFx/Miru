using Miru;
using Miru.PageTesting;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Accounts;
using Mong.Features.Home;
using Mong.PageTests.Fixtures;
using NUnit.Framework;

namespace Mong.PageTests.Pages.Accounts
{
    public class AccountLoginPageTest : PageTest
    {
        private User _user;

        public override void GivenSync()
        {
            _user = _.MakeSaving<User>(m => m.HashedPassword = Hash.Create("123456"));
        }
        
        [Test]
        public void Can_login_user()
        {
            _.Visit<AccountLogin>();

            _.Form<AccountLogin.Command>(f =>
            {
                f.Input(m => m.Email, _user.Email);
                f.Input(m => m.Password, "123456");
                f.Submit();
            });
            
            _.ShouldRedirectTo("/");
            _.ShouldHaveText(_user.Email);
            _.ShouldNotHaveText("Login");
        }

        [Test]
        public void Deny_wrong_credentials()
        {
            _.Visit<AccountLogin.Command>();

            _.Form<AccountLogin.Command>(f =>
            {
                f.Input(m => m.Email, _user.Email);
                f.Input(m => m.Password, "Wrong_Password");
                f.Submit();
            });
            
            _.ShouldNotHaveText("User and password not found");
        }
    }
}