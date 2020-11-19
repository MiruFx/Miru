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
    public class AreaEdit
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
            public Area Area { get; set; }
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
                var area = await _db.Areas.ByIdOrFailAsync(request.Id, ct);
                
                return new Command
                {
                    Id = area.Id,
                    Name = area.Name
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var area = await _db.Areas.ByIdOrFailAsync(request.Id, ct);

                area.Name = request.Name;
                
                return new Result { Area = area };
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
        
        public class AreasController : MiruController
        {
            [Route("/Areas/{id:long}/Edit")]
            public async Task<Command> AreaEdit(Query query) => await SendAsync(query);

            [HttpPost, Route("/Areas/Edit")]
            public async Task<Result> AreaEdit(Command command) => await SendAsync(command);
        }
    }
}
