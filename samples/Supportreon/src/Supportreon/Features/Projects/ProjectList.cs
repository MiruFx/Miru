using System;
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
    public class ProjectList
    {
        public class Query : IRequest<Query>
        {
            public bool Closed { get; set; }
            public string Category { get; set; }
            public string Search { get; set; }

            public IReadOnlyList<Project> Results { get; set; } = new List<Project>();
            public IReadOnlyList<Category> Categories { get; set; } = new List<Category>();
        }

        public class Handler : IRequestHandler<Query, Query>
        {
            private readonly SupportreonDbContext _db;
            
            public Handler(SupportreonDbContext db)
            {
                _db = db;
            }
            
            public async Task<Query> Handle(Query request, CancellationToken ct)
            {
                request.Results = await _db.Projects
                    .WhereWhen(request.Closed, p => DateTime.Now >= p.EndDate)
                    .WhereWhen(request.Closed == false, p => p.EndDate.HasValue == false || p.EndDate.Value.Date > DateTime.Today)
                    .WhereWhen(request.Category.NotEmpty(), p => p.Category.Name == request.Category)
                    .WhereWhen(request.Search.NotEmpty(), p => EF.Functions.Like(p.Name, $"%{request.Search}%"))
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync(ct);

                request.Categories = await _db.Categories.ToListAsync(ct);

                return request;
            }
        }
        
        public class ProjectsController : MiruController
        {
            [Route("/Projects")]
            public async Task<Query> List(Query request) => await SendAsync(request);
        }
    }
}
