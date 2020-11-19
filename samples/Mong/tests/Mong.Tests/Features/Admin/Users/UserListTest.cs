using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miru.Security;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Admin.Users;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Admin.Users
{
    public class UserListTest : OneCaseFeatureTest
    {
        private IEnumerable<User> _users;
        private User _admin;

        public override async Task GivenAsync()
        {
            _users = _.MakeMany<User>(2, m => m.IsAdmin = false);
            _admin = _.Make<User>(m => m.IsAdmin = true);
            
            await _.SaveAsync(_users, _admin);
        }

        [Test]
        public async Task Return_list_of_users()
        {
            _.LoginAs(_admin);
            
            var result = await _.SendAsync(new UserList.Query());
            
            result.CountTotal.ShouldBe(3);
        }
        
        [Test]
        public async Task Unauthorize_non_admin()
        {
            _.LoginAs(_users.First());

            await Should.ThrowAsync<UnauthorizedException>(async () => await _.SendAsync(new UserList.Query()));
        }
    }
}