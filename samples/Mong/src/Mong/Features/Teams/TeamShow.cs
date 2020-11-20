using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Mvc;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Teams
{
    public class TeamShow
    {
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {
            public Team Team { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly MongDbContext _db;
            
            public Handler(MongDbContext db)
            {
                _db = db;
            }
            
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return new Result
                {
                    Team = await _db.Teams.ByIdOrFailAsync(request.Id, ct)
                };
            }
        }
        
        public class TeamsController : MiruController
        {
            [Route("/Teams/{id:long}")]
            public async Task<Result> Show(Query request) => await SendAsync(request);
        }
    }
}
