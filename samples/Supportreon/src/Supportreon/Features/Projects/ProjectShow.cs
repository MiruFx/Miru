using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Mvc;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon.Features.Projects
{
    public class ProjectShow
    {
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {    
            public Project Project { get; set; }
            public IReadOnlyList<Donation> LastDonations { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly SupportreonDbContext _db;
            
            public Handler(SupportreonDbContext db)
            {
                _db = db;
            }
            
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                var project = await _db.Projects
                    .Include(m => m.User)
                    .ByIdOrFailAsync(request.Id, ct);

                var lastDonations = await _db.Donations
                    .Where(m => m.ProjectId == project.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Include(m => m.User)
                    .Take(5)
                    .ToListAsync(ct);
                
                return new Result
                {
                    Project = project,
                    LastDonations = lastDonations
                };
            }
        }
        
        public class ProjectsController : MiruController
        {
            [Route("/Projects/{Id:int}")]
            public async Task<Result> Show(Query request) => await SendAsync(request);
        }
    }
}
