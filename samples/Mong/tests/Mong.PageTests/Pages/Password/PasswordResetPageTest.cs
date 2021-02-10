using System.Threading.Tasks;
using Miru.PageTesting;
using Miru.Testing;
using Miru.Userfy;
using Mong.Features.Password;
using Mong.PageTests.Fixtures;
using NUnit.Framework;

namespace Mong.PageTests.Pages.Password
{
    public class PasswordResetPageTest : PageTest
    {
        private TopupBasic _fix;

        public override async Task GivenAsync()
        {
            _fix = await _.ScenarioAsync<TopupBasic>();
            
            // _fix.User.RequestedPasswordReset();
            
            await _.SaveAsync(_fix.User);
        }

        [Test]
        public void Can_reset_password()
        {
            _.Visit(new PasswordReset.Query { Token = _fix.User.ResetPasswordToken });

            _.Form<PasswordReset.Command>((f, command) =>
            {
                f.Input(m => m.Password, "123");
                f.Input(m => m.PasswordConfirmation, "123");
                f.Submit();
            });
            
            _.ShouldHaveText("Your password has been reset. Login with your new password");
        }
    }
}