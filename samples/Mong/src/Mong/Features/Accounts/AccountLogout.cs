using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Behaviors;
using Miru.Mvc;
using Miru.Userfy;

namespace Mong.Features.Accounts
{
    public class AccountLogout 
    {
        public class Command : IRequest<Command>, IRedirectResult
        {
            public string RedirectTo { get; set; }
        }
        
        public class Handler : IRequestHandler<Command, Command>
        {
            private readonly IUserSession _userSession;

            public Handler(IUserSession userSession) => _userSession = userSession;

            public async Task<Command> Handle(Command request, CancellationToken cancellationToken)
            {
                await _userSession.LogoutAsync(); 

                request.RedirectTo = "/";
                
                return request;
            }
        }

        public class AccountsController : MiruController
        {
            [HttpPost]
            public async Task<Command> Logout(Command command) => await SendAsync(command);
        }
    }
}