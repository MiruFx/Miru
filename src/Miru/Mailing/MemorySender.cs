using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;

namespace Miru.Mailing;

public class MemorySender : ISender
{
    private static readonly List<EmailSent> Emails = new();
        
    public EmailData Last() => Emails.Last().Data;
        
    public IEnumerable<EmailData> All() => Emails.Select(x => x.Data);

    public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
    {
        Emails.Add(new EmailSent { Data = email.Data });

        App.Framework
            .Information("Email sent to memory. From: {From}, To: {To}, Subject: {Subject}",
                email.Data.FromAddress.EmailAddress,
                email.Data.ToAddresses.Select(x => x.EmailAddress).Join(","),
                email.Data.Subject);
        
        return new SendResponse();
    }

    public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
    {
        return await Task.FromResult(Send(email));
    }

    public void Clear() => Emails.Clear();
}