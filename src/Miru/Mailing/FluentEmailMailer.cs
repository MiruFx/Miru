using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Miru.Queuing;

namespace Miru.Mailing;

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

        App.Framework.Information($"Sending email '{deliverableEmail.Subject}'");

        await _sender.SendAsync(new FluentEmail.Core.Email
        {
            Data = deliverableEmail
        });
            
        // TODO: close streams
    }
        
    /// <summary>
    /// It builds the email using all the passed parameters/services, then queue the only email message object 
    /// </summary>
    public async Task SendLaterAsync<TMailable>(
        TMailable mailable, 
        Action<Email> emailBuilder = null) where TMailable : Mailable
    {
        var fluentMail = await BuildEmailFromAsync(mailable, emailBuilder);

        Enqueue(fluentMail);
    }

    public async Task<Email> BuildEmailFromAsync<TMailable>(TMailable mailable, Action<Email> emailBuilder) 
        where TMailable : Mailable
    {
        mailable.MailingOptions = _options;

        var email = new Email();

        _options.SetDefaultEmail(email);

        // TODO: is okay both calls here (sync/async)?
        mailable.Build(email);
        await mailable.BuildAsync(email);
            
        emailBuilder?.Invoke(email);

        var validation = await new EmailValidator().ValidateAsync(email);

        if (validation.IsValid == false)
            throw new ValidationException(validation.Errors);
            
        // TODO: think about this part. email.Template is necessary only for 
        // generating string with email's html. after, we don't need it anymore
        // so it should not be serialized by the queue engine
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
            
        if (emailTemplate.TemplateName.StartsWith("_"))
            templateFile = $"{emailTemplate.TemplateName}.mail.cshtml";
        else
            templateFile = $"_{emailTemplate.TemplateName}.mail.cshtml";

        if (emailTemplate.TemplateAt.IsEmpty())
        {
            var type = mailable.GetType();

            var dirs = type.Namespace!.Replace(type.Assembly.GetName().Name!, string.Empty).Split('.', StringSplitOptions.RemoveEmptyEntries);

            emailTemplate.TemplateAt = string.Join(Path.DirectorySeparatorChar, dirs);
        }

        if (_options.TemplatePath.IsNotEmpty())
            return A.Path / _options.TemplatePath / emailTemplate.TemplateAt / templateFile;
            
        return A.Path / emailTemplate.TemplateAt / templateFile;
    }
        
    private void Enqueue(Email fluentMail)
    {
        _jobs.Enqueue(new EmailJob(fluentMail), queue: _options.QueueName);

        _logger.LogDebug(
            $"Enqueued email '{fluentMail.Subject}' to {fluentMail.ToAddresses.Select(m => m.EmailAddress).Join(",")}");
    }
}