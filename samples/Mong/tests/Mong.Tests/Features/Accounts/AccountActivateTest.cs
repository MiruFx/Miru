using System.Linq;
using System.Threading.Tasks;
using Baseline.Dates;
using FluentValidation.TestHelper;
using Miru;
using Miru.Domain;
using Miru.Testing;
using Mong.Database;
using Mong.Domain;
using Mong.Features.Accounts;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Accounts
{
    public class AccountActivateTest : FeatureTest
    {
        [Test]
        public async Task Can_activate_account()
        {
            // arrange
            var user = _.MakeSaving<User>();

            // act
            await _.SendAsync(new AccountActivate.Query
            {
                Token = user.ConfirmationToken
            });

            // assert
            var saved = _.Db(db => db.Users.First());
            
            saved.ConfirmationToken.ShouldBeNull();
            saved.ConfirmationSentAt.ShouldBeNull();
            saved.ConfirmedAt.ShouldBeSecondsAgo();
        }
        
        [Test]
        public void Throw_exception_if_cant_find_token()
        {
            Should.Throw<NotFoundException>(() => _.SendSync(new AccountActivate.Query { Token = "Token" }));
        }
    }
}