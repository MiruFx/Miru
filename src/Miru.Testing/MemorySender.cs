using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using Miru.Mailing;

namespace Miru.Testing
{
    public class MemorySender : ISender
    {
        private static readonly List<EmailSent> Emails = new List<EmailSent>();
        
        public EmailSent Last() => Emails.Last();
        
        public IEnumerable<EmailSent> All() => Emails;

        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            return SendAsync(email, token).GetAwaiter().GetResult();
        }

        public Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            var rawEmail = email.ToRawEmail();
            
            Emails.Add(new EmailSent { Data = email.Data });

            rawEmail.DumpToConsole("Email sent to memory: ");
            
            return Task.FromResult(new SendResponse());
        }
    }
}
