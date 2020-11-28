using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Miru;
using Miru.Domain;
using Miru.Mailing;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Accounts;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Accounts
{
    public class AccountRegisterTest : FeatureTest
    {
        [Test]
        public async Task Can_register_account()
        {
            // arrange
            var command = _.Make<AccountRegister.Command>();

            // act
            await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Users.First());

            saved.Email.ShouldBe(command.Email);
            saved.Name.ShouldBe(command.Name);
            saved.HashedPassword.ShouldBe(Hash.Create(command.Password));
            
            saved.ConfirmationSentAt.ShouldBeSecondsAgo();
            saved.ConfirmationToken.ShouldNotBeNull();

            var job = _.EnqueuedJob<EmailJob>();
            
            job.Email.ToAddresses.ShouldContain(m => m.EmailAddress == command.Email);
            job.Email.Subject.ShouldContain("Activate Your Mong Account");
            job.Email.Body.ShouldContain(saved.ConfirmationToken);
        }
        
        [Test]
        public async Task Email_should_be_unique()
        {
            // arrange
            var existentUser = _.MakeSaving<User>();
            var command = _.Make<AccountRegister.Command>(m => m.Email = existentUser.Email);

            // act & assert
            await Should.ThrowAsync<DomainException>(() => _.SendAsync(command));
        }

        public class Validations : ValidationTest<AccountRegister.Command>
        {
            [Test]
            public void Email_is_required_and_valid()
            {
                ShouldBeValid(m => m.Email, Request.Email);

                ShouldBeInvalid(m => m.Email, string.Empty);
                ShouldBeInvalid(m => m.Email, "admin!&admin");
            }

            [Test]
            public void Name_is_required()
            {
                ShouldBeValid(m => m.Name, Request.Name);

                ShouldBeInvalid(m => m.Name, string.Empty);
            }

            [Test]
            public void Password_is_required_and_should_match_confirmation()
            {
                ShouldBeValid(x => x.Password, Request);
                ShouldBeValid(x => x.PasswordConfirmation, Request);

                ShouldBeInvalid(m => m.PasswordConfirmation, string.Empty);
                ShouldBeInvalid(m => m.Password, string.Empty);
            }
        }
    }
}