using Miru.PageTesting;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Accounts;
using Mong.Features.Password;
using Mong.PageTests.Fixtures;
using NUnit.Framework;

namespace Mong.PageTests.Pages.Password
{
    public class PasswordForgotPageTest : PageTest
    {
        private User _user;

        public override void Given()
        {
            _user = _.MakeSaving<User>();
        }

        [Test]
        public void Can_email_link_to_reset_password()
        {
            _.Visit<PasswordForgot.Command>();

            _.Form<PasswordForgot.Command>((f, command) =>
            {
                f.Input(m => m.Email, _user.Email);
                f.Submit();
            });
            
            _.ShouldHaveText("An email for you has been sent with the link to reset your password");
        }
    }
}