using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Miru;
using Miru.Userfy;
using Mong.Database;

namespace Mong.Features.Accounts
{
    public class AccountActivate
    {
        public class Query : IRequest<Result>
        {
            public string Token { get; set; }
        }

        public class Result
        {
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly MongDbContext _db;

            public Handler(MongDbContext db)
            {
                _db = db;
            }

            // _body
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                var user =  await _db.Users
                    .Where(x => x.ConfirmationToken == request.Token)
                    .SingleOrFailAsync($"Could not find confirmation token {request.Token}", ct);

                user.ConfirmActivation();
                
                return new Result();
            }
            // _body
        }

        public class AccountsController
        {
            private readonly IMediator _mediator;
            
            public AccountsController(IMediator mediator) => _mediator = mediator;

            public async Task<Result> Activate(Query query) => await _mediator.Send(query);
        }
    }
}