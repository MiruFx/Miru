using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Userfy;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Admin.Topups
{
    public class TopupList
    {
        public class Query : IRequest<Query>, IPageable<Topup>
        {
            public IReadOnlyList<Topup> Results { get; set; }

            public long? ProviderId { get; set; }
            public TopupStatus? Status { get; set; }
            
            public int Page { get; set; }
            public int PageSize { get; set; } = 10;
            public int Pages { get; set; }
            public int CountShowing { get; set; }
            public int CountTotal { get; set; }
            
            public IEnumerable<Provider> Providers { get; set; }
            public IEnumerable<TopupStatus> Statuses => EnumEx.All<TopupStatus>();
            
            public string ProviderFilter => ProviderId.HasValue ? Providers?.ById(ProviderId.Value)?.Name : "All";
            public string StatusFilter => Status.HasValue ? Statuses.By(Status.Value).ToString() : "All";
        }

        public class Handler : IRequestHandler<Query, Query>
        {
            private readonly MongDbContext _db;
            private readonly IUserSession<User> _userSession;

            public Handler(MongDbContext db, IUserSession<User> userSession)
            {
                _db = db;
                _userSession = userSession;
            }

            public async Task<Query> Handle(Query request, CancellationToken ct)
            {
                request.Results = await _db.Topups
                    .WhereWhen(request.ProviderId.HasValue, m => m.ProviderId == request.ProviderId)
                    .WhereWhen(request.Status.HasValue, m => m.Status == request.Status)
                    .Include(m => m.Provider)
                    .Include(m => m.User)
                    .OrderByDescending(m => m.CreatedAt)
                    .ToPaginateAsync(request, ct);

                request.Providers = await _db.Providers.ToListAsync(ct);
                
                return request;
            }
        }
        
        [Route("Admin/Topups")]
        public class TopupsController : MiruController
        {
            public async Task<Query> List(Query request) => await SendAsync(request);
        }
    }
}