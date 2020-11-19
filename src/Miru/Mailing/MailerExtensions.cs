using System.Threading.Tasks;
using Miru.Domain;

namespace Miru.Mailing
{
    public static class MailerExtensions
    {
        public static async Task SendNowTo<TMailable>(
            this IMailer mailer, 
            IReceivesEmail to, 
            TMailable mailable) where TMailable : Mailable
        {
            await mailer.SendNowAsync(mailable, email =>
            {
                email.To(to.Email, to.Name);
            });
        }
        
        public static async Task SendLaterAsync<TMailable>(
            this IMailer mailer, 
            IReceivesEmail to, 
            TMailable mailable) where TMailable : Mailable
        {
            await mailer.SendLaterAsync(mailable, email =>
            {
                email.To(to.Email, to.Name);
            });
        }
    }
}