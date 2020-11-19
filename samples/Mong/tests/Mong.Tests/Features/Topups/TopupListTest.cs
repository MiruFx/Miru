using System.Collections.Generic;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Mong.Features.Topups;
using System.Linq;
using System.Threading.Tasks;
using Mong.Domain;

namespace Mong.Tests.Features.Topups
{
    public class TopupListTest : OneCaseFeatureTest
    {
        private User _user;
        private User _userWithNoTopups;
        private IEnumerable<Topup> _topups;

        public override async Task GivenAsync()
        {
            _user = _.Make<User>();
            _userWithNoTopups = _.Make<User>();

            _topups = _.MakeMany<Topup>(m => m.User = _user);

            await _.SaveAsync(_user, _userWithNoTopups, _topups);
        }

        [Test]
        public async Task Current_user_can_see_own_topups()
        {
            // arrange
            _.LoginAs(_user);

            // act
            var result = await _.SendAsync(new TopupList.Query());
           
            // assert
            result.Results.Count.ShouldBe(_topups.Count());
        }
        
        [Test]
        public async Task Other_users_cant_see_others_topup()
        {
            // arrange
            _.LoginAs(_userWithNoTopups);
            
            // act
            var result = await _.SendAsync(new TopupList.Query());
           
            // assert
            result.Results.Count.ShouldBe(0);
        }
    }
}