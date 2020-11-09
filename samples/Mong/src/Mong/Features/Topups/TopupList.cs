using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Miru.Mvc;
using Miru.Userfy;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Topups
{
    public class TopupList
    {
        public class Query : IRequest<Query>
        {
            public IReadOnlyList<Topup> Results { get; set; }
        }

        public class Handler : IRequestHandler<Query, Query>
        {
            private readonly MongDbContext _db;
            private readonly IUserSession<User> _currentUser;

            public Handler(MongDbContext db, IUserSession<User> currentUser)
            {
                _db = db;
                _currentUser = currentUser;
            }

            public async Task<Query> Handle(Query request, CancellationToken ct)
            {
                var user = await _currentUser.User();
                
                request.Results = await _db.Topups
                    .Where(m => m.UserId == user.Id)
                    .Include(m => m.Provider)
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync(ct);

                return request;
            }
        }
        
        public class TopupsController : MiruController
        {
            public async Task<Query> List(Query request) => await Send(request);
        }
    }
}