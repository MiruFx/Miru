using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Supportreon.Domain;
using Supportreon.Features.Accounts.Passwords;

namespace Supportreon.Tests.Features.Accounts.Passwords
{
    public class PasswordEditTest : FeatureTest
    {
        [Test]
        public async Task Can_edit_password()
        {
            // arrange
            var user = await _.MakeUserAsync<User>("CurrentPassword!1");

            _.LoginAs(user);
            
            // act
            await _.SendAsync(new PasswordEdit.Command
            {
                CurrentPassword = "CurrentPassword!1",
                NewPassword = "NewPassword1!",
                NewPasswordConfirmation = "NewPassword1!"
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
                ShouldBeValid(x => x.CurrentPassword, Request.CurrentPassword);

                ShouldBeInvalid(m => m.CurrentPassword, string.Empty);
            }
            
            [Test]
            public void New_password_is_required_and_should_match_confirmation()
            {
                Request.NewPassword = "NewPassword1!";
                Request.NewPasswordConfirmation = "NewPassword1!";
                
                ShouldBeValid(x => x.NewPassword, Request);
                ShouldBeValid(x => x.NewPasswordConfirmation, Request);

                ShouldBeInvalid(m => m.NewPassword, string.Empty);
                ShouldBeInvalid(m => m.NewPassword, "OtherPassword1!");
            }
        }
    }
}
