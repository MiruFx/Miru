using System.Linq;
using System.Threading.Tasks;
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
    public class PasswordEditTest : FeatureTest
    {
        [Test]
        public async Task User_can_edit_own_password()
        {
            // arrange
            var user = _.MakeSavingLogin<User>(m => m.HashedPassword = Hash.Create("123456"));

            // act
            await _.SendAsync(new PasswordEdit.Command
            {
                CurrentPassword = "123456",
                Password = "NewPassword",
                PasswordConfirmation = "NewPassword"
            });
            
            // assert
            var saved = _.Db(db => db.Users.First());
            saved.HashedPassword.ShouldBe(Hash.Create("NewPassword"));
        }

        [Test]
        public void Throw_exception_if_current_password_does_not_match()
        {
            // arrange
            _.MakeSavingLogin<User>(m => m.HashedPassword = Hash.Create("123456"));

            // act & assert
            Should.Throw<DomainException>(() =>
                _.SendSync(new PasswordEdit.Command
                {
                    CurrentPassword = "TotallyWrongPassword",
                    Password = "NewPassword",
                    PasswordConfirmation = "NewPassword"
                }));
        }
        
        public class Validations : ValidationTest<PasswordEdit.Command>
        {
            [Test]
            public void Current_password_is_required()
            {
                ShouldBeValid(x => x.Password, Request);

                ShouldBeInvalid(m => m.Password, string.Empty);
            }
            
            [Test]
            public void Password_is_required_and_should_match_confirmation()
            {
                ShouldBeValid(x => x.Password, Request);
                ShouldBeValid(x => x.PasswordConfirmation, Request);

                ShouldBeInvalid(m => m.Password, "else");
            }
        }
    }
}