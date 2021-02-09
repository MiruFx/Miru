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
                var product = new Team
                {
                    Name = request.Name
                };

                await _db.Teams.AddAsync(product, ct);

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
        
        public class ProductsController : MiruController
        {
            [Route("/Products/New")]
            public async Task<Command> New(Query query) => await SendAsync(query);

            [HttpPost, Route("/Products/New")]
            public async Task<Result> New(Command command) => await SendAsync(command);
        }
    }
}