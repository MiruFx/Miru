using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Teams
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
        }

        public class Handler : 
            IRequestHandler<Query, Command>, 
            IRequestHandler<Command, Result>
        {
            private readonly MongDbContext _db;
            
            public Handler(MongDbContext db)
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

                return new Result();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotEmpty();
            }
        }
        
        public class TeamsController : MiruController
        {
            [Route("/Teams/New")]
            public async Task<Command> New(Query query) => await SendAsync(query);

            [HttpPost, Route("/Teams/New")]
            public async Task<Result> New(Command command) => await SendAsync(command);
        }
    }
}
