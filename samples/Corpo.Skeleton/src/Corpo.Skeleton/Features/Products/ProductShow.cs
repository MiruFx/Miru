using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Database;
using Corpo.Skeleton.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Products
{
    public class ProductShow
    {
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {
            public Product Product { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly SkeletonDbContext _db;
            
            public Handler(SkeletonDbContext db)
            {
                _db = db;
            }
            
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return new Result
                {
                    Product = await _db.Products.ByIdOrFailAsync(request.Id, ct)
                };
            }
        }
        
        public class ProductsController : MiruController
        {
            [Route("/Products/{id:long}")]
            public async Task<Result> Show(Query request) => await SendAsync(request);
        }
    }
}