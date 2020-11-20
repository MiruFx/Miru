using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;
using Mong.Database;

namespace Mong.Features.Teams
{
    public class TeamEdit
    {
        public class Query : IRequest<Command>
        {
            public long Id { get; set; }
        }
        
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
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
            
            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var team = await _db.Teams.ByIdOrFailAsync(request.Id, ct);
                
                return new Command
                {
                    Id = team.Id,
                    Name = team.Name
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var team = await _db.Teams.ByIdOrFailAsync(request.Id, ct);

                team.Name = request.Name;
                
                return new Result();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
                
                RuleFor(m => m.Name).NotEmpty();
            }
        }
        
        public class TeamsController : MiruController
        {
            [Route("/Teams/{id:long}/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost, Route("/Teams/Edit")]
            public async Task<Result> Edit(Command command) => await SendAsync(command);
        }
    }
}
