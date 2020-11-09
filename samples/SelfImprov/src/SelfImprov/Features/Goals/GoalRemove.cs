using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Databases.EntityFramework;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Features.Goals
{
    public class GoalRemove
    {
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {
            public long Id { get; set; }
        }

        public class Handler : 
            IRequestHandler<Command, Result>
        {
            private readonly SelfImprovDbContext _db;
            
            public Handler(SelfImprovDbContext db)
            {
                _db = db;
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                await _db.Goals.InactiveByIdAsync(request.Id, ct);

                return new Result { Id = request.Id };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
            }
        }
        
        public class GoalsController : MiruController
        {
            [HttpPost, Route("/Goals/Remove/{Id}")]
            public async Task<Result> Remove(Command command) => await Send(command);
        }
    }
}
