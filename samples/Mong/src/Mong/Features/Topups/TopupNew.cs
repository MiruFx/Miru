using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HtmlTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Mvc;
using Miru.Queuing;
using Miru.Security;
using Miru.Userfy;
using Mong.Database;
using Mong.Domain;
using Mong.Payments;

namespace Mong.Features.Topups
{
    public class TopupNew : IMustBeAuthenticated
    {
        public class Query : IRequest<Command>
        {
        }
        
        public class Command : IRequest<Command>, IBelongsToUser
        {
            public string PhoneNumber { get; set; }
            public decimal Amount { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public long UserId { get; set; }
            public string CreditCard { get; set; }
            public long ProviderId { get; set; }
            
            public IReadOnlyList<ILookupable> Providers { get; set; } = new List<Provider>();
        }

        public class Handler : 
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Command>
        {
            private readonly IUserSession _userSession;
            private readonly MongDbContext _db;
            private readonly IPayPau _payPau;
            private readonly Jobs _jobs;

            public Handler(MongDbContext db, IPayPau payPau, Jobs jobs, IUserSession userSession)
            {
                _userSession = userSession;
                _db = db;
                _payPau = payPau;
                _jobs = jobs;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var user = await _db.Users.ByIdAsync(_userSession.CurrentUserId, ct);
                
                return new Command
                {
                    Providers = _db.Providers.ToList(),
                    Name = user.Name,
                    Email = user.Email
                };
            }
            
            public async Task<Command> Handle(Command request, CancellationToken ct)
            {
                App.Log.Debug("Retrieving entities from db");
                
                var user = await _db.Users.ByIdAsync(request.UserId, ct);
                
                var provider = await _db.Providers.ByIdOrFailAsync(request.ProviderId, ct);
                
                App.Log.Debug("Creating and paying a new Topup");
                
                var topup = new Topup(request, user, provider);
                
                topup.Pay(_payPau, request.CreditCard);
                
                App.Log.Debug("Save db and enqueue job");
                
                await _db.AddSavingAsync(topup);
                
                _jobs.PerformLater(new TopupComplete { TopupId = topup.Id });
                
                App.Log.Information($"Successful created Topup #{topup.Id}");

                return request;
            }
        }
        
        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(m => m.ProviderId).NotEmpty();
                
                RuleFor(m => m.PhoneNumber).NotEmpty();

                RuleFor(m => m.Amount).NotEmpty().GreaterThanOrEqualTo(5);
                
                RuleFor(m => m.CreditCard).NotEmpty();
            }
        }
        
        public class TopupsController : MiruController
        {
            private readonly MongDbContext _db;

            public TopupsController(MongDbContext db) => _db = db;

            public async Task<Command> New(Query request) => await SendAsync(request);
            
            [HttpPost]
            public async Task<Command> New(Command request) => await SendAsync(request);

            public async Task<HtmlTag> Amounts(AmountByProvider query)
            {
                return (await _db.Providers.ByIdAsync(query.ProviderId))?
                    .AllAmounts()
                    .ToSelectOptionsTags();
            }
        }

        public class AmountByProvider
        {
            public long ProviderId { get; set; }
        }
    }
}