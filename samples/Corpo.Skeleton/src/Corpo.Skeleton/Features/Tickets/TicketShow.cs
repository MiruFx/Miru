using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Tickets
{
    public class TicketShow
    {
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {
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
                return new Result();
            }
        }
        
        public class Controller : MiruController
        {
            [HttpGet("/Tickets/{id:long}")]
            public async Task<Result> Show(Query request) => await SendAsync(request);
        }
    }
}