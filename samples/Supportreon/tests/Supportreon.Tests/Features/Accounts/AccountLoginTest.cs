using System.Threading.Tasks;
using Miru.Testing;
using Supportreon.Domain;
using Supportreon.Features.Accounts;
using NUnit.Framework;
using Shouldly;

namespace Supportreon.Tests.Features.Accounts
{
    public class AccountLoginTest : FeatureTest
    {
        [Test]
        public async Task Can_login()
        {
            // arrange
            var user = _.MakeUser<User>(password: "Password1!");

            await _.SaveAsync(user);

            // act
            var result = await _.SendAsync(new AccountLogin.Command
            {
                Email = user.Email,
                Password = "Password1!"
            });

            // assert
            result.SignInResult.Succeeded.ShouldBeTrue();
            _.CurrentUserId().ShouldBe(user.Id);
        }

        public class Validations : ValidationTest<AccountLogin.Command>
        {
            [Test]
            public void Email_is_required_and_valid()
            {
                ShouldBeValid(m => m.Email, Request.Email);

                ShouldBeInvalid(m => m.Email, string.Empty);
                ShouldBeInvalid(m => m.Email, "admin!.admin");
            }

            [Test]
            public void Password_is_required()
            {
                ShouldBeValid(x => x.Password, Request.Password);

                ShouldBeInvalid(m => m.Password, string.Empty);
            }
        }
    }
}
