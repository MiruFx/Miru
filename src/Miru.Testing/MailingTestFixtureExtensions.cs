using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core.Models;
using Miru.Mailing;
using Shouldly;

namespace Miru.Testing
{
    public static class MailingTestFixtureExtensions
    {
        public static async Task SendEmailNowAsync<TMail>(this ITestFixture fixture, TMail mail) where TMail : Mailable
        {
            using (var scope = fixture.App.WithScope())
            {
                var mailer = scope.Get<IMailer>();
                await mailer.SendNowAsync(mail);
            }
        }
        
        public static void ShouldContainEmail(this IEnumerable<Address> address, string email)
        {
            address.ShouldContain(
                m => m.EmailAddress == email, 
                $"Should contain {email} but there were: {address.Select(x => x.EmailAddress).Join(",")}");
        }
    }
}