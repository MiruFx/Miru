using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Miru.Queuing;

namespace Supportreon.Features.Donations
{
    public class DonationRecurringChargeJob : IJob
    {
        public class Handler : IRequestHandler<DonationRecurringChargeJob>
        {
            private readonly ILogger<DonationRecurringChargeJob> _logger;

            public Handler(ILogger<DonationRecurringChargeJob> logger)
            {
                _logger = logger;
            }

            public Task<Unit> Handle(DonationRecurringChargeJob request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("DonationRecurringChargeJob performed ....");
                
                /*
                 * - Retrieve all Donations with IsRecurrent = true
                 * - Performs the billing operation
                 * - Maybe send any email confirmation
                 * 
                 */
                
                return Unit.Task;
            }
        }
    }
}