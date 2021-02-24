using System.IO;
using System.Linq;
using System.Text;
using FluentEmail.Core;

namespace Miru.Mailing
{
    public static class FluentMailExtensions
    {
        public static string ToRawEmail(this IFluentEmail email)
        {
            var emailMessage = new StringBuilder();
            
            emailMessage.Append($@"
<pre>
From: {email.Data.FromAddress.Name} <{email.Data.FromAddress.EmailAddress}>
To: {string.Join(",", email.Data.ToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}
Cc: {string.Join(",", email.Data.CcAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}
Bcc: {string.Join(",", email.Data.BccAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}
ReplyTo: {string.Join(",", email.Data.ReplyToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}
Subject: {email.Data.Subject}
</pre>");

            foreach (var dataHeader in email.Data.Headers)
            {
                emailMessage.Append($"{dataHeader.Key}:{dataHeader.Value}");
            }

            emailMessage.AppendLine();

            emailMessage.Append(email.Data.Body);

            return emailMessage.ToString();
        }
    }
}