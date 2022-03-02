using System.Text;
using Corpo.Skeleton.Features.Accounts.Passwords;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Corpo.Skeleton.Tests.Features.Accounts.Passwords;

public class PasswordResetTest : FeatureTest
{
    [Test]
    public async Task Can_query_for_password_reset()
    {
        // arrange
        var user = await _.MakeUserAsync<User>();
        var code = await CreateResetCode(user);

        // act
        var command = await _.SendAsync(new PasswordReset.Query
        {
            Code = code
        });

        // assert
        command.Code.ShouldBe(Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)));
    }
        
    [Test]
    public async Task Can_reset_password()
    {
        // arrange
        var user = await _.MakeUserAsync<User>();
        var code = await CreateResetCode(user);

        // act
        await _.SendAsync(new PasswordReset.Command
        {
            Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
            Email = user.Email,
            Password = "NewPassword1!",
            PasswordConfirmation = "NewPassword1!"
        });

        // assert
        var saved = _.Db(db => db.Users.First());
        saved.PasswordHash.ShouldNotBe(user.PasswordHash);
    }

    private async Task<string> CreateResetCode(User user)
    {
        using var scope = _.WithScope();
            
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(
            await scope.Get<UserManager<User>>().GeneratePasswordResetTokenAsync(user)));

        return code;
    }
        
    public class Validations : ValidationTest<PasswordReset.Command>
    {
        [Test]
        public void Email_is_required_and_valid()
        {
            ShouldBeValid(Request, m => m.Email, Request.Email);

            ShouldBeInvalid(Request, m => m.Email, string.Empty);
            ShouldBeInvalid(Request, m => m.Email, "admin!.admin");
        }
            
        [Test]
        public void Code_is_required()
        {
            ShouldBeValid(Request, m => m.Code, Request.Code);

            ShouldBeInvalid(Request, m => m.Code, string.Empty);
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