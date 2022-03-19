using Miru.PageTesting;
using NUnit.Framework;
using Playground.Features.Accounts;

namespace Playground.PageTests.Pages.Accounts;

public class AccountRegisterPageTest : PageTest
{
    [Test]
    public void Can_register_new_user()
    {
        // new FirefoxDriver()
        // var browser = new FirefoxDriver(new FirefoxOptions());
        //
        // var navigator = new ChromeNavigator(
        //     _.Get<WebDriverWait>(),
        //     _.Get<ElementNaming>(),
        //     () => browser.FindElement(By.TagName("body")),
        //     browser,
        //     _.Get<ILogger<MiruNavigator>>());
        
        _.Visit<AccountRegister.Command>();
        
        _.Form<AccountRegister.Command>((f, command) =>
        {
            f.Input(m => m.Email, command.Email);
            f.Input(m => m.Password, "123");
            f.Input(m => m.PasswordConfirmation, "123");
            f.Submit();
        });
            
        _.ShouldHaveText("Your account has been created!");
    }
}