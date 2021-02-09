using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Miru.Core;
using Miru.Queuing;

namespace Miru.Mailing
{
    public class FluentEmailMailer : IMailer
    {
        private readonly ISender _sender;
        private readonly MailingOptions _options;
        private readonly Jobs _jobs;
        private readonly ILogger<IMailer> _logger;
        private readonly RazorViewToStringRenderer _razorRenderer;

        public FluentEmailMailer(
            ISender sender, 
            MailingOptions options, 
            Jobs jobs, 
            ILogger<IMailer> logger, 
            RazorViewToStringRenderer razorRenderer)
        {
            _sender = sender;
            _options = options;
            _jobs = jobs;
            _logger = logger;
            _razorRenderer = razorRenderer;
        }
        
        public async Task SendNowAsync<TMailable>(TMailable mailable, Action<Email> emailBuilder = null) where TMailable : Mailable
        {
            var deliverableEmail = await BuildEmailFromAsync(mailable, emailBuilder);

            await _sender.SendAsync(new FluentEmail.Core.Email { Data = deliverableEmail });
            
            // TODO: if too many email, show: 'email1, email2 and more 300'
            _logger.LogDebug($"Sent email '{deliverableEmail.Subject}' to {deliverableEmail.ToAddresses.Select(m => m.EmailAddress).Join(",")}");
        }
        
        /// <summary>
        /// It builds the email using all the passed parameters/services, then queue the only email message object 
        /// </summary>
        public async Task SendLaterAsync<TMailable>(TMailable mailable, Action<Email> emailBuilder = null) where TMailable : Mailable
        {
            var fluentMail = await BuildEmailFromAsync(mailable, emailBuilder);

            Enqueue(fluentMail);
        }

        public async Task<Email> BuildEmailFromAsync<TMailable>(TMailable mailable, Action<Email> emailBuilder) where TMailable : Mailable
        {
            mailable.MailingOptions = _options;

            var email = new Email();

            _options.SetDefaultEmail(email);

            mailable.Build(email);
            
            emailBuilder?.Invoke(email);

            if (email.Template != null)
            {
                var fullFile = GetFullFile(mailable, email.Template);
                
                var body = await _razorRenderer.RenderViewToStringAsync(fullFile, email.Template.Model);

                email.Body = body;
                email.IsHtml = true;
            }

            return email;
        }

        private string GetFullFile(Mailable mailable, EmailTemplate emailTemplate)
        {
            string templateFile;
            
            if (emailTemplate.File.StartsWith("_"))
                templateFile = $"{emailTemplate.File}.mail.cshtml";
            else
                templateFile = $"_{emailTemplate.File}.mail.cshtml";

            var type = mailable.GetType();

            var dirs = type.Namespace!.Replace(type.Assembly.GetName().Name!, string.Empty).Split('.', StringSplitOptions.RemoveEmptyEntries);

            var path = string.Join(Path.DirectorySeparatorChar, dirs);

            if (_options.TemplatePath.IsNotEmpty())
                return A.Path(_options.TemplatePath) / path / templateFile;
            
            return A.Path(path) / templateFile;
        }
        
        private void Enqueue(Email fluentMail)
        {
            _jobs.PerformLater(new EmailJob(fluentMail));

            _logger.LogDebug(
                $"Enqueued email '{fluentMail.Subject}' to {fluentMail.ToAddresses.Select(m => m.EmailAddress).Join(",")}");
        }
    }
}