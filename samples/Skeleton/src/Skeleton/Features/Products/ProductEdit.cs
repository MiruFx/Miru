using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;
using Skeleton.Database;

namespace Skeleton.Features.Products
{
    public class ProductEdit
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
            private readonly SkeletonDbContext _db;
            
            public Handler(SkeletonDbContext db)
            {
                _db = db;
            }
            
            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var product = await _db.Products.ByIdOrFailAsync(request.Id, ct);
                
                return new Command
                {
                    Id = product.Id,
                    Name = product.Name
                };
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var product = await _db.Products.ByIdOrFailAsync(request.Id, ct);

                product.Name = request.Name;
                
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
        
        public class ProductsController : MiruController
        {
            [Route("/Products/{id:long}/Edit")]
            public async Task<Command> Edit(Query query) => await Send(query);

            [HttpPost, Route("/Products/Edit")]
            public async Task<Result> Edit(Command command) => await Send(command);
        }
    }
}