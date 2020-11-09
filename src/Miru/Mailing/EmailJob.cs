using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Miru.Queuing;

namespace Miru.Mailing
{
    public class EmailJob : IJob
    {
        public Email Email { get; }
        
        public EmailJob(Email email)
        {
            Email = email;
        }

        public class Handler : IRequestHandler<EmailJob>
        {
            private readonly FluentEmail.Core.Interfaces.ISender _sender;
            private readonly ILogger<Handler> _logger;

            public Handler(FluentEmail.Core.Interfaces.ISender sender, ILogger<Handler> logger)
            {
                _sender = sender;
                _logger = logger;
            }

            public async Task<Unit> Handle(EmailJob request, CancellationToken cancellationToken)
            {
                var response = await _sender.SendAsync(new FluentEmail.Core.Email { Data = request.Email });
            
                if (response.Successful)
                    _logger.LogDebug($"Email '{request.Email.Subject}' sent to {request.Email.ToAddresses.Select(m => m.EmailAddress).Join(",")}");

                return Unit.Value;
            }
        }
    }
}