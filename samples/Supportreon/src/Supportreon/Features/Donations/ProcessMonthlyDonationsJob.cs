using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Miru;
using Miru.Queuing;
using Supportreon.Database;

namespace Supportreon.Features.Donations
{
    public class ProcessMonthlyDonationsJob : IMiruJob
    {
        public  long DonationId { get; init; }

        public class Handler : IRequestHandler<ProcessMonthlyDonationsJob>
        {
            private readonly SupportreonDbContext _db;
            private readonly ILogger<ProcessMonthlyDonationsJob> _logger;

            public Handler(SupportreonDbContext db, ILogger<ProcessMonthlyDonationsJob> logger)
            {
                _db = db;
                _logger = logger;
            }

            public async Task<Unit> Handle(ProcessMonthlyDonationsJob request, CancellationToken ct)
            {
                var donation = await _db.Donations.ByIdOrFailAsync(request.DonationId, ct);
                
                //Perform donation
                _logger.LogInformation($"Was charged the donation with id {donation.Id}");

                return await Unit.Task;
            }
        }
    }
}