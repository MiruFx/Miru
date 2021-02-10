using System.Linq;
using Baseline.Dates;
using FluentValidation;
using Miru;
using Miru.Domain;
using Miru.Mailing;
using Miru.Testing;
using Miru.Userfy;
using Mong.Domain;
using Mong.Features.Password;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Password
{
    public class PasswordResetTest : FeatureTest
    {
        [Test]
        public void Can_query_to_reset_password()
        {
            // arrange
            // var user = _.MakeSaving<User>(m => m.RequestedPasswordReset());
            //
            // // act
            // var command = _.SendSync(new PasswordReset.Query
            // {
            //     Token = user.ResetPasswordToken
            // });
            //
            // // assert
            // command.Token.ShouldBe(user.ResetPasswordToken);
        }

        [Test]
        public void Can_reset_password()
        {
            // arrange
            var user = _.MakeSaving<User>(m => m.ResetPasswordToken = "Token");
            
            // act
            _.SendSync(new PasswordReset.Command
            {
                Token = user.ResetPasswordToken,
                Password = "321",
                PasswordConfirmation = "321"
            });

            // assert
            var saved = _.Db(db => db.Users.First());
            saved.HashedPassword.ShouldBe(Hash.Create("321"));

            var job = _.EnqueuedJob<EmailJob>();
            
            job.Email.ToAddresses.ShouldContain(m => m.EmailAddress == user.Email);
            job.Email.Subject.ShouldContain("Reset Password");
            job.Email.Body.ShouldContain("Your password has been reset.");
        }

        [Test]
        public void Cannot_reset_password_with_not_existent_reset_token()
        {
            // arrange
            var user = _.MakeSaving<User>(m =>
            {
                m.ResetPasswordToken = "Token";
                m.ResetPasswordSentAt = 1.Months().Ago();
            });
            
            // act, assert
            Should.Throw<ValidationException>(() => _.SendSync(new PasswordReset.Query
            {
                Token = user.ResetPasswordToken
            }));
        }
        
        [Test]
        public void Cannot_reset_password_with_expired_reset_token()
        {
            Should.Throw<NotFoundException>(() => _.SendSync(_.Make<PasswordReset.Query>()));
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