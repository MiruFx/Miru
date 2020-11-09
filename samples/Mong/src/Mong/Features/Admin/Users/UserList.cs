using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Security;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Admin.Users
{
    public class UserList
    {
        // #query
        public class Query : IRequest<Query>, IPageable<User>
        {
            public string Name { get; set; }
            public string Email { get; set; }

            public IReadOnlyList<User> Results { get; set; }
            
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Pages { get; set; }
            public int CountShowing { get; set; }
            public int CountTotal { get; set; }
        }
        // #query
        
        public class Handler : IRequestHandler<Query, Query>
        {
            private readonly MongDbContext _db;

            public Handler(MongDbContext db)
            {
                _db = db;
            }

            public async Task<Query> Handle(Query request, CancellationToken ct)
            {
                request.Results = await _db.Users.ToPaginateAsync(request, ct);
                
                return request;
            }
        }
        
        [Route("Admin/Users")]
        public class UsersController : MiruController
        {
            public async Task<Query> List(Query request) => await Send(request);
        }
    }
}