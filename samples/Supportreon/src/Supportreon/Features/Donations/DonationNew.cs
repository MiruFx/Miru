using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Domain;
using Miru.Mvc;
using Miru.Security;
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

            public Handler(SupportreonDbContext db)
            {
                _db = db;
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
                var project = await _db.Projects.ByIdOrFailAsync(request.ProjectId, ct);

                var donation = new Donation(request, project);

                await _db.Donations.AddAsync(donation, ct);

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
                RuleFor(m => m.ProjectId).NotEmpty();
                
                RuleFor(m => m.Amount).NotEmpty();
                
                RuleFor(m => m.CreditCard).NotEmpty();
            }
        }
        
        public class DonationsController : MiruController
        {
            [Route("/Projects/{ProjectId:long}/Donations/New")]
            public async Task<Command> New(Query query) => await SendAsync(query);

            [HttpPost, Route("/Projects/{ProjectId:long}/Donations/New")]
            public async Task<Result> New(Command command) => await SendAsync(command);
        }
    }
}
