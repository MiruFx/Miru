using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Mvc;
using Miru.Security;
using Supportreon.Database;
using Miru;

namespace Supportreon.Features.Projects
{
    public class ProjectNew : IMustBeAuthenticated
    {
        public class Query : IRequest<ProjectEdit.Command>
        {
        }
        
        public class Handler : IRequestHandler<Query, ProjectEdit.Command>
        {
            private readonly SupportreonDbContext _db;

            public Handler(SupportreonDbContext db)
            {
                _db = db;
            }

            public async Task<ProjectEdit.Command> Handle(Query request, CancellationToken ct)
            {
                return new ProjectEdit.Command
                {
                    Categories = await _db.Categories.ToLookupableAsync(ct)
                };
            }
        }
        
        public class ProjectsController : MiruController
        {
            public async Task<ProjectEdit.Command> New(Query query) => await SendAsync(query);
        }
    }
}
