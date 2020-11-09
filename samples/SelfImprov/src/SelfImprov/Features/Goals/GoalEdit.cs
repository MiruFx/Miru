using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Features.Goals
{
    public class GoalEdit
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
            public Goal Goal { get; set; }
        }

        public class Handler : 
            IRequestHandler<Query, Command>, 
            IRequestHandler<Command, Result>
        {
            private readonly SelfImprovDbContext _db;
            
            public Handler(SelfImprovDbContext db)
            {
                _db = db;
            }
            
            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var goal = await _db.Goals.ByIdOrFailAsync(request.Id, ct);
                
                return new Command
                {
                    Id = goal.Id,
                    Name = goal.Name
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var goal = await _db.Goals.ByIdOrFailAsync(request.Id, ct);

                goal.Name = request.Name;
                
                return new Result { Goal = goal };
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
        
        public class GoalsController : MiruController
        {
            [Route("/Goals/{id:long}/Edit")]
            public async Task<Command> Edit(Query query) => await Send(query);

            [HttpPost, Route("/Goals/Edit")]
            public async Task<Result> Edit(Command command) => await Send(command);
        }
    }
}
