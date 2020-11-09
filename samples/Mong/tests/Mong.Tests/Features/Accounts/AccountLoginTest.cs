using System;
using System.Threading.Tasks;
using Miru;
using Miru.Domain;
using Miru.Testing;
using Miru.Userfy;
using Mong.Domain;
using Mong.Features.Accounts;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Accounts
{
    public class AccountLoginTest : FeatureTest
    {
        [Test]
        public async Task Can_login()
        {
            // arrange
            var user = _.MakeSaving<User>(m =>
            {
                m.ConfirmedAt = DateTime.Now;
                m.HashedPassword = Hash.Create("123456");
            });
            var command = new AccountLogin.Command
            {
                Email = user.Email,
                Password = "123456"
            };

            // act
            await _.Send(command);

            // assert
            _.Get<IUserSession>().CurrentUserId.ShouldBe(user.Id);
        }

        [Test]
        public async Task User_not_confirmed_registration_cannot_login()
        {
            // arrange
            var user = _.MakeSaving<User>(m =>
            {
                m.ConfirmedAt = null;
                m.HashedPassword = Hash.Create("123456");
            });
            
            var command = new AccountLogin.Command
            {
                Email = user.Email,
                Password = "123456"
            };

            // act
            await Should.ThrowAsync<NotFoundException>(async () => await _.Send(command));

            // assert
            _.Get<IUserSession>().IsLogged.ShouldBeFalse();
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