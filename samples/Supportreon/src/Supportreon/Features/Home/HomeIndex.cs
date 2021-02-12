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

namespace Supportreon.Features.Home
{
    public class HomeIndex
    {
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {
            public IReadOnlyList<Project> Projects { get; set; }
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
                return new Result
                {
                    Projects = await _db.Projects
                        .OrderByDescending(m => m.CreatedAt)
                        .Take(5)
                        .ToListAsync(ct)
                };
            }
        }
        
        public class HomeController : MiruController
        {
            [Route("/")]
            public async Task<Result> Index(Query request) => await SendAsync(request);
            
            [Route("Error/{code?}")]
            public IActionResult Error(int? code)
            {
                if (code == 404) return View("404");
                if (code == 403) return View("403");
                
                return View(code);
            }
        }
    }
}
