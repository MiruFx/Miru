using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Playground.Domain;
using Playground.Features.Accounts.Passwords;
using Shouldly;

namespace Playground.Tests.Features.Accounts.Passwords;

public class PasswordEditTest : FeatureTest
{
    [Test]
    public async Task Can_edit_password()
    {
        // arrange
        var user = await _.MakeUserAsync<User>("Password123456!");

        _.LoginAs(user);
            
        // act
        await _.SendAsync(new PasswordEdit.Command
        {
            CurrentPassword = "Password123456!",
            NewPassword = "NewPassword123456!",
            NewPasswordConfirmation = "NewPassword123456!"
        });

        // assert
        var saved = _.Db(db => db.Users.First());
        saved.PasswordHash.ShouldNotBe(user.PasswordHash);
    }

    public class Validations : ValidationTest<PasswordEdit.Command>
    {
        [Test]
        public void Current_password_is_required()
        {
            ShouldBeValid(Request, x => x.CurrentPassword, Request.CurrentPassword);

            ShouldBeInvalid(Request, m => m.CurrentPassword, string.Empty);
        }
            
        [Test]
        public void New_password_is_required_and_should_match_confirmation()
        {
            Request.NewPassword = "NewPassword123456!";
            Request.NewPasswordConfirmation = "NewPassword123456!";
                
            ShouldBeValid(Request, x => x.NewPassword);
            ShouldBeValid(Request, x => x.NewPasswordConfirmation);

            ShouldBeInvalid(Request, m => m.NewPassword, string.Empty);
            ShouldBeInvalid(Request, m => m.NewPassword, "OtherPassword1!");
        }
    }
}