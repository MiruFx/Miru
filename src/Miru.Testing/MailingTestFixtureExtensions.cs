using System.Threading.Tasks;
using Miru.Mailing;

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
    }
}