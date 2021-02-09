using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;
using Miru.Userfy;

namespace Corpo.Skeleton.Features.Accounts
{
    public class AccountLogout 
    {
        public class Command : IRequest<Result>
        {
            public string RedirectTo { get; set; }
        }
        
        public class Result : IRedirect
        {
            public object RedirectTo { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IUserSession _userSession;

            public Handler(IUserSession userSession)
            {
                _userSession = userSession;
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                await _userSession.LogoutAsync();

                return new Result
                {
                    RedirectTo = "/"
                };
            }
        }

        public class AccountsController : MiruController
        {
            [HttpPost]
            public async Task<Result> Logout(Command command) => await SendAsync(command);
        }
    }
}