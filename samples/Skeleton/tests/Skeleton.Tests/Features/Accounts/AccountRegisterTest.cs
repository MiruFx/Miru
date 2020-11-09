using System.Linq;
using System.Threading.Tasks;
using Miru;
using Miru.Mailing;
using Miru.Testing;
using Skeleton.Domain;
using Skeleton.Features.Accounts;
using NUnit.Framework;
using Shouldly;

namespace Skeleton.Tests.Features.Accounts
{
    public class AccountRegisterTest : FeatureTest
    {
        [Test]
        public async Task Can_register_account()
        {
            // arrange
            var command = _.Make<AccountRegister.Command>();

            // act
            await _.Send(command);

            // assert
            var saved = _.Db(db => db.Users.First());

            saved.Email.ShouldBe(command.Email);
            saved.Name.ShouldBe(command.Name);
            saved.HashedPassword.ShouldBe(Hash.Create(command.Password));
            
            var job = _.EnqueuedRawJob<EmailJob>();
            job.ShouldContain(command.Email);
            job.ShouldContain("Welcome To Skeleton");
        }

        public class Validations : ValidationTest<AccountRegister.Command>
        {
            [Test]
            public void Email_is_required_and_valid_and_unique()
            {
                var existentUser = _.MakeSaving<User>();

                ShouldBeValid(m => m.Email, Request.Email);

                ShouldBeInvalid(m => m.Email, existentUser.Email);
                ShouldBeInvalid(m => m.Email, string.Empty);
                ShouldBeInvalid(m => m.Email, "admin!.admin");
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