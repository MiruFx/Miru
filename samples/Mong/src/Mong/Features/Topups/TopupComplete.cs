using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru;
using Miru.Core;
using Miru.Mailing;
using Miru.Queuing;
using Mong.Database;
using Mong.Domain;
using Mong.Payments;

namespace Mong.Features.Topups
{
    public class TopupComplete : IJob
    {
        public long TopupId { get; set; }

        public class Handler : IRequestHandler<TopupComplete>
        {
            private readonly IMailer _mailer;
            private readonly IMobileProvider _mobileProvider;
            private readonly MongDbContext _db;

            public Handler(IMailer mailer, IMobileProvider mobileProvider, MongDbContext db)
            {
                _mailer = mailer;
                _mobileProvider = mobileProvider;
                _db = db;
            }

            public async Task<Unit> Handle(TopupComplete request, CancellationToken cancellationToken)
            {    
                var topup = _db.Topups.ByIdOrFail(request.TopupId);

                if (topup.Status != TopupStatus.Paid)
                {
                    // This scenario is not expected unless something went wrong
                    // In a real app, could send email to user telling something went wrong and to pay again
                    App.Log.Information($"Topup #{topup.Id} is not paid. It will not be processed");
                    
                    return Unit.Value;
                }

                // "Synchronously" ask mobile provider to add credit to the mobile number
                // if topup successfully, save topup and send email to the user
                try
                {
                    _mobileProvider.Topup(topup.PhoneNumber, topup.Amount);
                    
                    topup.Complete();
                
                    await _mailer.SendNowAsync(new TopupCompletedMail(topup));
                }
                catch (Exception ex)
                {
                    App.Log.Error($"Failed to topup #{topup.Id} with phone number {topup.PhoneNumber}", ex);

                    topup.Error();
                    
                    await _mailer.SendNowAsync(new TopupFailedMail(topup));
                }

                return Unit.Value;
            }
            
            public class TopupCompletedMail : Mailable
            {
                private readonly Topup _topup;

                public TopupCompletedMail(Topup topup)
                {
                    _topup = topup;
                }

                public override void Build(Email mail)
                {
                    mail.To(_topup.Email, _topup.Name)
                        .Subject("Topup Successful")
                        .Template(string.Empty, _topup);
                }
            }
            
            public class TopupFailedMail : Mailable
            {
                private readonly Topup _topup;

                public TopupFailedMail(Topup topup)
                {
                    _topup = topup;
                }

                public override void Build(Email mail)
                {
                    mail.To(_topup.Email, _topup.Name)
                        .Subject("Topup Failed")
                        .Template(string.Empty, _topup);
                }
            }
        }
    }
}