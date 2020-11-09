using Miru.Mailing;
using Miru.Testing;
using Mong.Domain;
using Mong.Features.Password;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests.Features.Password
{
    public class PasswordForgotMailTest : MiruTest
    {
        /// <summary>
        /// Check if mail is sent
        /// Check if content is correct
        /// Check when is delivering 
        /// </summary>
        [Test]
        public void Reset_password_mail()
        {
            // arrange
            var user = _.Make<User>(m => m.ResetPasswordToken = "Token");

            // act
            _.Get<IMailer>().SendNow(new PasswordForgot.PasswordForgotMail(user));
            
            // assert
            var emailSent = _.LastEmailSent().Data;
            emailSent.Body.ShouldContain($"Hello, {user.Name}");
            emailSent.Body.ShouldContain(user.ResetPasswordToken);
        }
    }
}