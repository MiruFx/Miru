using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Queuing;

namespace Miru.Mailing;

public class EmailJob : MiruJob<EmailJob>
{
    public Email Email { get; }

    public override string Id => Email.Subject;
        
    public EmailJob(Email email)
    {
        Email = email;
    }

    public class Handler : IRequestHandler<EmailJob, EmailJob>
    {
        private readonly FluentEmail.Core.Interfaces.ISender _sender;

        public Handler(FluentEmail.Core.Interfaces.ISender sender)
        {
            _sender = sender;
        }

        public async Task<EmailJob> Handle(EmailJob request, CancellationToken cancellationToken)
        {
            var response = await _sender.SendAsync(new FluentEmail.Core.Email { Data = request.Email });
            
            if (response.Successful)
                App.Framework.Information(
                    "Email {Subject} sent to {ToAddresses}", 
                    request.Email.Subject,
                    request.Email.ToAddresses.Select(m => m.EmailAddress).Join(","));

            return request;
        }
    }
}