using FluentEmail.Core.Models;

namespace Miru.Mailing
{
    public static class EmailExtensions
    {
        public static EmailData From(this Email emailData, string email, string name = "")
        {
            emailData.FromAddress = new Address(email, name);
            
            return emailData;
        }

        public static EmailData ReplyTo(this Email emailData, string email, string name = "")
        {
            emailData.ReplyToAddresses.Add(new Address(email, name));
            
            return emailData;
        }

        public static Email To(this Email emailData, string email, string name = null)
        {
            emailData.ToAddresses.Add(new Address(email, name));
            
            return emailData;
        }

        public static Email Subject(this Email emailData, string subject)
        {
            emailData.Subject = subject;
            
            return emailData;
        }
        
        public static Email Template(this Email email, string template, object model = null, bool isHtml = true)
        {
            email.Template = new EmailTemplate
            {
                File = template,
                Model = model,
                IsHtml = isHtml
            };
            
            return email;
        }
        
        public static Email Body(this Email emailData, string body)
        {
            emailData.Body = body;
            
            return emailData;
        }
    }
}