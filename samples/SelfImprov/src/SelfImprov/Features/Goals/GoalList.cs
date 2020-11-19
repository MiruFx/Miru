using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;
using Z.EntityFramework.Plus;

namespace SelfImprov.Features.Goals
{
    public class GoalList
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public IReadOnlyList<Area> Areas = new List<Area>();
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
                    Areas = await _db.Areas
                        .IncludeGoals()
                        .ToListAsync(ct)
                };
            }
        }
        
        public class GoalsController : MiruController
        {
            [Route("/Goals")]
            public async Task<Result> List(Query request) => await SendAsync(request);
        }
    }
}
