using System.Threading.Tasks;
using Miru.Security;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Accounts;
using Mong.Features.Admin.Users;
using Mong.Features.Topups;
using Mong.Tests;
using NUnit.Framework;
using Shouldly;

namespace Miru.AcceptanceTests.Authorization
{
    public class CanTest : OneCaseFeatureTest
    {
        private User _admin;
        private User _customer;

        public override async Task GivenAsync()
        {
            // arrange
            _admin = _.Fab().Users.Admin().Make();
            _customer = _.Fab().Users.Customer().Make();

            await _.SaveAsync(_admin, _customer);
        }

        [Test]
        public async Task Admin_can_see_list_of_users()
        {
            _.LoginAs(_admin);

            var result = await _.Get<Authorizer>().Can<UserList.Query>();
                
            result.ShouldBeTrue();
        }
        
        [Test]
        public async Task Customer_cannot_see_list_of_users()
        {
            _.LoginAs(_customer);

            var result = await _.Get<Authorizer>().Can<UserList.Query>();
                
            result.ShouldBeFalse();
        }
        
        [Test]
        public async Task Anonymous_cannot_see_list_of_users()
        {
            _.Logout();

            var result = await _.Get<Authorizer>().Can<UserList.Query>();
                
            result.ShouldBeFalse();
        }
        
        [Test]
        public async Task Anonymous_can_login()
        {
            _.Logout();

            var result = await _.Get<Authorizer>().Can<AccountLogin.Query>();
                
            result.ShouldBeTrue();
        }
        
        [Test]
        public async Task Anonymous_cant_topup()
        {
            _.Logout();

            var result = await _.Get<Authorizer>().Can<TopupNew.Query>();
                
            result.ShouldBeFalse();
        }
    }
}