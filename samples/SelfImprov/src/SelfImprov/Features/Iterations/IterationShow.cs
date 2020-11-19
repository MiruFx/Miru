using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Features.Iterations
{
    public class IterationShow
    {
        // #query
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }
        // #query
        
        public class Result
        {
            public Iteration Iteration { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly SelfImprovDbContext _db;
            
            public Handler(SelfImprovDbContext db)
            {
                _db = db;
            }
            
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return new Result
                {
                    Iteration = await _db.Iterations
                        .Where(i => i.Id == request.Id)
                        .Include(i => i.Achievements)
                        .ThenInclude(a => a.Goal)
                        .ThenInclude(g => g.Area)
                        //.Include(i => i.Achievements.Select(a => a.Goal.Area))
                        //.OrderBy(i => i.Achievements.OrderByDescending(a => a.Achieved).Select(a => a.Achieved).FirstOrDefault())
                        
                        // TODO: SingleOrFail
                        .SingleOrDefaultAsync(i => i.Id == request.Id, ct)
                };
            }
        }
        
        public class IterationsController : MiruController
        {
            [Route("/Iterations/{id:long}")]
            public async Task<Result> Show(Query request) => await SendAsync(request);
        }
    }
}
