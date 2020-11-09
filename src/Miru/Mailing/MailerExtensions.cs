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
            await mailer.SendNow(mailable, email =>
            {
                email.To(to.Email, to.Name);
            });
        }
        
        public static async Task SendLaterTo<TMailable>(
            this IMailer mailer, 
            IReceivesEmail to, 
            TMailable mailable) where TMailable : Mailable
        {
            await mailer.SendLater(mailable, email =>
            {
                email.To(to.Email, to.Name);
            });
        }
    }
}