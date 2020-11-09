using Miru.PageTesting;
using Mong.Features.Accounts;
using NUnit.Framework;

namespace Mong.PageTests.Pages.Accounts
{
    public class AccountRegisterPageTest : PageTest
    {
        [Test]
        public void Can_register_new_user()
        {
            _.Visit<AccountRegister.Command>();

            _.Form<AccountRegister.Command>((f, command) =>
            {
                f.Input(m => m.Name, command.Name);
                f.Input(m => m.Email, command.Email);
                f.Input(m => m.Password, "123");
                f.Input(m => m.PasswordConfirmation, "123");
                f.Submit();
            });
            
            _.ShouldHaveText("An email has been sent to you with a confirmation link to activate your account");
        }
    }
}