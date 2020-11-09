using System;
using FluentEmail.Core.Models;

namespace Miru.Mailing
{
    public class MailingOptions
    {
        private Action<Email> _defaultEmailAction;

        public string AppUrl { get; set; }
        public SmtpOptions Smtp { get; set; } = new SmtpOptions();
        public string TemplatePath { get; set; }

        public void EmailDefaults(Action<Email> defaultMail)
        {
            _defaultEmailAction = defaultMail;
        }

        public EmailData SetDefaultEmail(Email email)
        {
            _defaultEmailAction?.Invoke(email);
            return email;
        }
    }
}