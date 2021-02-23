using System.Linq;
using System.Threading.Tasks;
using Miru.Domain;
using Miru.Mailing;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Supportreon.Domain;
using Supportreon.Features.Accounts;
using Supportreon.Features.Accounts.Passwords;

namespace Supportreon.Tests.Features.Accounts.Passwords
{
    public class PasswordForgotTest : FeatureTest
    {
        [Test]
        public async Task Can_generate_reset_email()
        {
            // arrange
            var user = _.MakeUser<User>();

            // act
            await _.SendAsync(new PasswordForgot.Command
            {
                Email = user.Email
            });

            // assert
            var job = _.EnqueuedJob<EmailJob>();
            job.Email.ToAddresses.ShouldContain(user.Email);
            job.Email.Body.ShouldContain("password reset");
        }

        public class Validations : ValidationTest<AccountRegister.Command>
        {
            [Test]
            public void Email_is_required_and_valid()
            {
                ShouldBeValid(m => m.Email, Request.Email);

                ShouldBeInvalid(m => m.Email, string.Empty);
                ShouldBeInvalid(m => m.Email, "admin!.admin");
            }
        }
    }
}
