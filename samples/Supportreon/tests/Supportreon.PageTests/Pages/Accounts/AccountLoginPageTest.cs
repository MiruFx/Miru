using System.Threading.Tasks;
using Miru.PageTesting;
using Miru.Testing;
using Supportreon.Features.Accounts;
using NUnit.Framework;
using Supportreon.Domain;
using Supportreon.Features.Home;
using Supportreon.Tests;

namespace Supportreon.PageTests.Pages.Accounts
{
    public class AccountLoginPageTest : PageTest
    {
        private User _user;

        public override async Task GivenAsync()
        {
            _user = await _.MakeUserAsync<User>(SupportreonFabricator.Password);
        }

        [Test]
        public void Can_login()
        {
            // arrange
            _.Visit<AccountLogin>();

            // act
            _.Form<AccountLogin.Command>(f =>
            {
                f.Input(m => m.Email, _user.Email);
                f.Input(m => m.Password, SupportreonFabricator.Password);
                f.Submit();
            });
            
            // assert
            _.ShouldRedirectTo<HomeIndex>();
        }
    }
}
