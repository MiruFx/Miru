using System;
using System.Threading.Tasks;
using FluentEmail.Core.Models;

namespace Miru.Mailing;

public interface IMailer
{
    /// <summary>
    /// Builds and send email now
    /// </summary>
    Task SendNowAsync<TMailable>(
        TMailable mailable, 
        Action<Email> emailBuilder = null) where TMailable : Mailable;

    // /// <summary>
    // /// Builds and send email now
    // /// </summary>
    // Task SendNowAsync(Email email);
        
    /// <summary>
    /// It builds the email using all the passed parameters/services, then queue the only email message object 
    /// </summary>
    Task SendLaterAsync<TMailable>(
        TMailable mailable, 
        Action<Email> emailBuilder = null) where TMailable : Mailable;

    // /// <summary>
    // /// It builds the email using all the passed parameters/services, then queue the only email message object 
    // /// </summary>
    // Task SendLaterAsync(Email email);
}