using System.Threading.Tasks;
using Miru.Domain;
using Miru.Testing;
using NUnit.Framework;
using Playground.Domain;
using Playground.Features.Accounts;
using Shouldly;

namespace Playground.Tests.Features.Accounts;

public class AccountLoginTest : FeatureTest
{
    [Test]
    public async Task Should_login_existent_user()
    {
        // arrange
        var user = await _.MakeUserAsync<User>(password: "Password1!");

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
        
    [Test]
    public async Task Should_not_login_inexistent_user()
    {
        // arrange
        // act
        // assert
        await Should.ThrowAsync<DomainException>(() => _.SendAsync(new AccountLogin.Command
        {
            Email = "doesnt@exist.com",
            Password = "Password1!"
        }));
    }
        
    public class Validations : ValidationTest<AccountLogin.Command>
    {
        [Test]
        public void Email_is_required_and_valid()
        {
            ShouldBeValid(Request, m => m.Email, Request.Email);

            ShouldBeInvalid(Request, m => m.Email, string.Empty);
            ShouldBeInvalid(Request, m => m.Email, "admin!.admin");
        }

        [Test]
        public void Password_is_required()
        {
            ShouldBeValid(Request, x => x.Password, Request.Password);

            ShouldBeInvalid(Request, m => m.Password, string.Empty);
        }
    }
}