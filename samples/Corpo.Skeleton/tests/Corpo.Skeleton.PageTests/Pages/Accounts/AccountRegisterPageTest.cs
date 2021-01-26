using Corpo.Skeleton.Features.Accounts;
using Miru.PageTesting;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Accounts
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
            
            _.ShouldHaveText("Your account has been created!");
        }
    }
}