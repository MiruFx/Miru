using System.Linq;
using System.Threading.Tasks;
using Baseline.Dates;
using Miru;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Password;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Password
{
    public class PasswordForgotTest : FeatureTest
    {
        [Test]
        public async Task Should_save_reset_token_and_send_email()
        {
            // arrange
            var user = _.MakeSaving<User>();

            // act
            await _.SendAsync(new PasswordForgot.Command
            {
                Email = user.Email
            });
            
            // assert
            _.EnqueuedCount().ShouldBe(1);
            
            var saved = _.Db(db => db.Users.First());

            saved.Email.ShouldBe(user.Email);
            saved.ResetPasswordToken.ShouldNotBeEmpty();
            saved.ResetPasswordSentAt.ShouldBeSecondsAgo();
        }

        public class Validations : OneCaseFeatureTest
        {
            private PasswordForgot.Command _request;

            public override void Given()
            {
                _request = _.Fab().Make<PasswordForgot.Command>();   
            }

            [Test]
            public void Email_is_required_and_valid()
            {
                _.ShouldBeValid(_request, x => x.Email, _.Faker().Internet.Email());

                _.ShouldBeInvalid(_request, x => x.Email, string.Empty);
                _.ShouldBeInvalid(_request, x => x.Email, "admin@admin");
            }
        }
    }
}