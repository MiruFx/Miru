using System;
using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Features.Admin.Users;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Admin.Users
{
    public class UserBlockTest : FeatureTest, IRequiresAuthenticatedAdmin
    {
        [Test]
        public async Task Block_user()
        {
            // arrange
            var user = _.MakeSaving<User>(m => m.BlockedAt = null);
            
            // act
            await _.SendAsync(new UserBlockUnblock.Command { UserId = user.Id });
            
            // assert
            var saved = _.Db(db => db.Users.First(m => m.Id == user.Id));
            saved.IsBlocked().ShouldBeTrue();
        }
        
        [Test]
        public async Task Unblock_user()
        {
            // arrange
            var user = _.MakeSaving<User>(m => m.BlockedAt = DateTime.Today);
            
            // act
            await _.SendAsync(new UserBlockUnblock.Command { UserId = user.Id });
            
            // assert
            var saved = _.Db(db => db.Users.First(m => m.Id == user.Id));
            saved.IsBlocked().ShouldBeFalse();
        }
    }
}