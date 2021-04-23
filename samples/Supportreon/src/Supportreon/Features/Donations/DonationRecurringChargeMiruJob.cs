using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Miru.Queuing;
using Supportreon.Database;

namespace Supportreon.Features.Donations
{
    public class DonationRecurringChargeMiruJob : IMiruJob
    {
        public class DonationRecurringChargeHandler : IRequestHandler<DonationRecurringChargeMiruJob>
        {
            private readonly SupportreonDbContext _db;
            private ILogger<DonationRecurringChargeMiruJob> _logger;

            public DonationRecurringChargeHandler(SupportreonDbContext db, ILogger<DonationRecurringChargeMiruJob> logger)
            {
                _db = db;
                _logger = logger;
            }

            public async Task<Unit> Handle(DonationRecurringChargeMiruJob request, CancellationToken cancellationToken)
            {
                var recurrentDonations = await _db.Donations
                    .Where(m => m.IsRecurrent)
                    .ToListAsync(cancellationToken);

                recurrentDonations.ForEach(c =>
                {
                    //Charge the donation
                    _logger.LogInformation($"Was charged the donation with id {c.Id}");
                });

                return await Unit.Task;
            }
        }
        }
    
}