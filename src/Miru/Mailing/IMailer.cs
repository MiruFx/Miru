using System;
using System.Threading.Tasks;
using FluentEmail.Core.Models;

namespace Miru.Mailing
{
    public interface IMailer
    {
        /// <summary>
        /// Builds and send email now
        /// </summary>
        Task SendNow<TMailable>(TMailable mailable, Action<Email> emailBuilder = null) where TMailable : Mailable;
        
        /// <summary>
        /// It builds the email using all the passed parameters/services, then queue the only email message object 
        /// </summary>
        Task SendLater<TMailable>(TMailable mailable, Action<Email> emailBuilder = null) where TMailable : Mailable;
    }
}