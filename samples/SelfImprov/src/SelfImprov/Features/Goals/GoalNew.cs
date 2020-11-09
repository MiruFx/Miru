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
    public class GoalNew
    {
        public class Query : IRequest<Command>
        {
            public long AreaId { get; set; }
        }
        
        // #command
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public long AreaId { get; set; }
        }
        // #command
        
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
            
            public Task<Command> Handle(Query request, CancellationToken ct)
            {
                return Task.FromResult(new Command
                {
                    AreaId = request.AreaId
                });
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var goal = new Goal
                {
                    Name = request.Name,
                    AreaId = request.AreaId
                };

                await _db.Goals.AddAsync(goal, ct);

                await _db.SaveChangesAsync(ct);
                
                return new Result { Goal = goal };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotEmpty();
                
                RuleFor(m => m.AreaId).NotEmpty();
            }
        }
        
        public class GoalsController : MiruController
        {
            [Route("/Areas/{AreaId}/Goals/New")]
            public async Task<Command> New(Query query) => await Send(query);

            [HttpPost, Route("/Goals/New")]
            public async Task<Result> New(Command command) => await Send(command);
        }
    }
}
