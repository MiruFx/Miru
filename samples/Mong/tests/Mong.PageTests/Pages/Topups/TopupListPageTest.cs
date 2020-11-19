using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miru;
using Miru.PageTesting;
using Miru.Testing;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Features.Admin.Users;
using Mong.Features.Topups;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Mong.PageTests.Pages.Topups
{
    public class TopupListPageTest : PageTest, IRequiresAuthenticatedUser
    {
        private IEnumerable<Topup> _topups;

        public override void Given()
        {
            _topups = _.MakeManySaving<Topup>(m => m.UserId = _.CurrentUserId());
            
            _.Visit(new TopupList.Query());
        }

        [Test]
        public void Can_see_topups()
        {
            _.Display<TopupList.Query>(table =>
            {
                table.ShouldHave(m => m.Results[0].PhoneNumber, _topups.Last().PhoneNumber);
            });
        }
    }
}