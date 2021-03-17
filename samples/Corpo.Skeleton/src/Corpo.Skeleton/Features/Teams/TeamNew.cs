using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Database;
using Corpo.Skeleton.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Teams
{
    public class TeamNew
    {
        public class Query : IRequest<Command>
        {
        }
        
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
        }

        public class Result
        {
            public Team Team { get; set; }
        }

        public class Handler : 
            IRequestHandler<Query, Command>, 
            IRequestHandler<Command, Result>
        {
            private readonly SkeletonDbContext _db;
            
            public Handler(SkeletonDbContext db)
            {
                _db = db;
            }
            
            public Task<Command> Handle(Query request, CancellationToken ct)
            {
                return Task.FromResult(new Command());
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var team = new Team
                {
                    Name = request.Name
                };

                await _db.Teams.AddAsync(team, ct);

                return new Result
                {
                    Team = team
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotEmpty();
            }
        }
        
        public class Controller : MiruController
        {
            [HttpGet("/Teams/New")]
            public async Task<Command> New(Query query) => await SendAsync(query);

            [HttpPost("/Teams/New")]
            public async Task<Result> New(Command command) => await SendAsync(command);
        }
    }
}