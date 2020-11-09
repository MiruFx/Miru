using System.Threading;
using System.Threading.Tasks;
using Baseline;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Mvc;
using SelfImprov.Database;

namespace SelfImprov.Features.Goals
{
    public class AreaRemove
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
                var area = await _db.Areas
                    .Include(m => m.Goals)
                    .ByIdOrFailAsync(request.Id, ct);

                area.IsInactive = true;
                area.Goals.Each(m => m.IsInactive = true);
                
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
        
        public class AreasController : MiruController
        {
            [HttpPost, Route("/Areas/Remove/{Id}")]
            public async Task<Result> AreaRemove(Command command) => await Send(command);
        }
    }
}
