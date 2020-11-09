using System.Collections.Generic;
using Miru;
using Miru.PageTesting;
using Miru.Testing;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Features.Admin.Users;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Mong.PageTests.Pages.Admin.Users
{
    public class UserListPageTest : PageTest, IRequiresAuthenticatedAdmin
    {
        private IEnumerable<User> _users;

        public override void GivenSync()
        {
            _users = _.MakeManySaving<User>();
            
            _.Visit(new UserList.Query());
        }

        [Test]
        public void Can_see_users()
        {
            _.Within(By.Id($"user-{_users.At(0).Id}"), row =>
            {
                row.ShouldHave(By.Id("Email"), _users.At(0).Email);
            });
        }

        [Test]
        public void Can_block_user()
        {
            // the div "user-ID" was replaced by a new one with same name
            // that's why the assert has to be done out of this within
            _.Within(By.Id($"user-{_users.At(0).Id}"), row =>
            {
                row.ClickLink("Block");
            });
            
            _.Within(By.Id($"user-{_users.At(0).Id}"), row =>
            {
                _.ShouldHaveText("Unblock");
            });
        }
    }
}