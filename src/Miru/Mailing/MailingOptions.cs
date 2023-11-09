using FluentEmail.Core.Models;

namespace Miru.Mailing;

public class MailingOptions
{
    private Action<Email> _defaultEmailAction;

    public string AppUrl { get; set; }
    public SmtpOptions Smtp { get; set; } = new();
    public string TemplatePath { get; set; }
    public string QueueName { get; set; }
    
    public void EmailDefaults(Action<Email> defaultMail)
    {
        _defaultEmailAction = defaultMail;
    }

    public EmailData SetDefaultEmail(Email email)
    {
        _defaultEmailAction?.Invoke(email);
        return email;
    }

    public void EnqueueIn(string queueName)
    {
        QueueName = queueName;
    }
}