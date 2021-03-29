using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Domain;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Security;
using Miru.Userfy;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon.Features.Donations
{
    public class DonationNew : IMustBeAuthenticated
    {
        public record Query : IRequest<Command>
        {
            public long ProjectId { get; init; }
        }
        
        public class Command : IRequest<Result>
        {
            public long ProjectId { get; set; }
            public decimal Amount { get; set; }
            public string CreditCard { get; set; }
            
            public Project Project { get; set; }
        }

        public class Result
        {
            public Project Project { get; set; }
        }

        public class Handler : 
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Result>
        {
            private readonly SupportreonDbContext _db;
            private readonly IMailer _mailer;
            private readonly IUserSession<User> _userSession;

            public Handler(SupportreonDbContext db, IMailer mailer, IUserSession<User> userSession)
            {
                _db = db;
                _mailer = mailer;
                _userSession = userSession;
            }
            
            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var project = await _db.Projects.ByIdOrFailAsync(request.ProjectId, ct);
                
                return new Command
                {
                    ProjectId = request.ProjectId,
                    Project = project,
                    Amount = project.MinimumDonation
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                // here is commented the steps a Handle usually does
                
                // fetch all entities or data. fail fast if some data doesn't exist
                var donor = await _db.Users.ByIdAsync(_userSession.CurrentUserId, ct);
                var project = await _db.Projects.ByIdOrFailAsync(request.ProjectId, ct);

                // create the entities, call entities' methods to set properties or to do domain validations
                var donation = new Donation(request, project, donor);

                // save entities, send emails, queue works or any work
                await _db.Donations.AddAsync(donation, ct);
                await _mailer.SendLaterAsync(new ThankMail(donation));

                return new Result
                {
                    Project = project
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                // validator validates only if the request has valid properties
                // it doesn't invoke database or other services calls
                
                RuleFor(m => m.ProjectId).NotEmpty();
                
                RuleFor(m => m.Amount).NotEmpty();
                
                RuleFor(m => m.CreditCard).NotEmpty();
            }
        }
        
        public class DonationsController : MiruController
        {
            // query is when the browser sends GET
            // miru will respond with a view with the action's name
            // in this case New.cshtml
            [HttpGet("/Projects/{ProjectId:long}/Donations/New")]
            public async Task<Command> New(Query query) => await SendAsync(query);

            // command is when the browser sends POST, PUT, DELETE or PATCH
            // miru will respond with a Turbo partial view with the action's name
            // in this case: _New.turbo.cshtml
            [HttpPost("/Projects/{ProjectId:long}/Donations/New")]
            public async Task<Result> New(Command command) => await SendAsync(command);
        }
        
        public class ThankMail : Mailable
        {
            private readonly Donation _donation;

            public ThankMail(Donation donation)
            {
                _donation = donation;
            }

            public override void Build(Email mail)
            {
                mail.To(_donation.User.Email, _donation.User.Name)
                    .Subject("Thank you!")
                    .Template("_Thank", _donation);
            }
        }
    }
}
