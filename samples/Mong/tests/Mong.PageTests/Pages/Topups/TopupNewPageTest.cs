using System.Linq;
using System.Threading.Tasks;
using Miru;
using Miru.PageTesting;
using Miru.Testing;
using Mong.Features.Accounts;
using NUnit.Framework;
using Mong.Features.Topups;
using Mong.PageTests.Fixtures;
using Mong.Tests;
using Shouldly;

namespace Mong.PageTests.Pages.Topups
{
    public class TopupNewPageTest : PageTest
    {
        private TopupBasic _fix;

        public override async Task Given()
        {
            _fix = await _.Scenario<TopupBasic>();
        }

        [Test]
        public void Can_make_new_topup()
        {
            _.LoginAs(_fix.User);
            
            _.Visit<TopupNew.Query>();

            var command = _.Make<TopupNew.Command>();
            
            _.Form<TopupNew.Command>(f =>
            {
                f.Select(m => m.ProviderId, _fix.Provider.Name);
                f.Input(m => m.PhoneNumber, command.PhoneNumber);
                f.Select(m => m.Amount, _fix.Provider.AllAmounts().Second());
                f.Input(m => m.CreditCard, command.CreditCard);
                
                f.Submit();
            });
            
            _.ShouldHaveText("Topup successful");
        }
        
        [Test]
        public void Anonymous_should_be_redirected_to_login()
        {
            _.Logout();
            
            _.Visit<TopupNew>();
            
            _.ShouldRedirectTo<AccountLogin>();
            _.ShouldHaveText("Please, login or create a new account");
        }
    }
}