using Corpo.Skeleton.Features.Accounts;
using Miru.Mailing;

namespace Corpo.Skeleton.Tests.Features.Accounts;

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
        saved.PasswordHash.ShouldNotBe(command.Password);
            
        var job = _.EnqueuedJob<EmailJob>();
        job.Email.ToAddresses.ShouldContain(m => m.EmailAddress == command.Email);
        job.Email.Body.ShouldContain("Welcome To Corpo.Skeleton");
    }

    public class Validations : ValidationTest<AccountRegister.Command>
    {
        [Test]
        public void Email_is_required_and_valid_and_unique()
        {
            ShouldBeValid(Request, m => m.Email, Request.Email);

            ShouldBeInvalid(Request, m => m.Email, string.Empty);
            ShouldBeInvalid(Request, m => m.Email, "admin!.admin");
        }

        [Test]
        public void Password_is_required_and_should_match_confirmation()
        {
            ShouldBeValid(Request, x => x.Password);
            ShouldBeValid(Request, x => x.PasswordConfirmation);

            ShouldBeInvalid(Request, m => m.PasswordConfirmation, string.Empty);
            ShouldBeInvalid(Request, m => m.Password, string.Empty);
        }
    }
}