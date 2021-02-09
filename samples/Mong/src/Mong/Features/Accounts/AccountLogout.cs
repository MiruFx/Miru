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
        
        public class Handler : RequestHandler<Command, Command>
        {
            private readonly IUserSession _userSession;

            public Handler(IUserSession userSession) => _userSession = userSession;

            protected override Command Handle(Command request)
            {
                _userSession.Logout(); 

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