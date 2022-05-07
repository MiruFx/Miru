using System.IO;
using System.Threading.Tasks;
using FluentEmail.Core.Models;
using MimeTypes;

namespace Miru.Mailing;

public static class EmailExtensions
{
    public static Email From(this Email emailData, string email, string name = "")
    {
        emailData.FromAddress = new Address(email, name);
            
        return emailData;
    }

    public static Email ReplyTo(this Email emailData, string email, string name = "")
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
            TemplateName = template,
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
    
    public static Email TemplateAt(this Email email, string at, string template, object model = null, bool isHtml = true)
    {
        email.Template = new EmailTemplate
        {
            TemplateAt = at,
            TemplateName = template,
            Model = model,
            IsHtml = isHtml
        };
            
        return email;
    }
    
    public static Email Attach(
        this Email email, 
        string name, 
        Stream stream)
    {
        var mimeType = MimeTypeMap.GetMimeType(Path.GetExtension(name));
        
        email.Attachments.Add(new Attachment
        {
            Data = stream,
            Filename = name,
            ContentType = mimeType
        }); 
        
        return email;
    }
}