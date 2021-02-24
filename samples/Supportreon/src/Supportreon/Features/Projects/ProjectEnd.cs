using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon.Features.Projects
{
    public class ProjectEnd
    {
        public class Query : IRequest<Command>
        {
            public long Id { get; set; }
        }
        
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
            public Project Project { get; set; }
        }

        public class Result
        {
            public long Id { get; set; }
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
                var project = await _db.Projects.ByIdOrFailAsync(request.Id, ct);
                
                return new Command
                {
                    Id = project.Id,
                    Project = project
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var project = await _db.Projects.ByIdOrFailAsync(request.Id, ct);

                project.EndProject();

                return new Result
                {
                    Id = project.Id
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
            }
        }
        
        public class ProjectsController : MiruController
        {
            [Route("/Projects/{id:long}/End")]
            public async Task<Command> End(Query query) => await SendAsync(query);

            [HttpPost, Route("/Projects/{id:long}/End")]
            public async Task<Result> End(Command command) => await SendAsync(command);
        }
    }
}
