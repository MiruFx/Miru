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

namespace SelfImprov.Features.Iterations
{
    public class IterationList
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {    
            public IReadOnlyList<Iteration> Iterations { get; set; } = new List<Iteration>();
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly SelfImprovDbContext _db;

            public Handler(SelfImprovDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query query, CancellationToken ct)
            {
                return new Result
                {
                    Iterations = await _db.Iterations
                        .Include(i => i.Achievements)
                        .OrderByDescending(i => i.Id)
                        .ToListAsync(ct)
                };
            }
        }
        
        public class IterationsController : MiruController
        {
            [Route("/Iterations")]
            public async Task<Result> List(Query request) => await Send(request);
        }
    }
}
