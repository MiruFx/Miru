using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Mvc;
using Miru.Userfy;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Supportreon.Features.Accounts
{
    public class AccountLogin
    {
        public class Query : IRequest<Command>
        {
            public string ReturnUrl { get; set; }
        }
        
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
            
            public string ReturnUrl { get; set; }
        }

        public class Result : IRedirect
        {
            public SignInResult SignInResult { get; set; }
            public object RedirectTo { get; set; }
        }
        
        public class Handler :
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Result>
        {
            private readonly IUserSession _userSession;

            public Handler(IUserSession userSession)
            {
                _userSession = userSession;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                request.ReturnUrl ??= "/";

                // Clear the existing external cookie to ensure a clean login process
                await _userSession.LogoutAsync();
                
                return new Command
                {
                    ReturnUrl = request.ReturnUrl
                };
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                request.ReturnUrl ??= "/";

                var result = await _userSession.LoginAsync(
                    request.Email, 
                    request.Password, 
                    request.RememberMe);
                
                if (result.Succeeded)
                {
                    return new Result
                    {
                        RedirectTo = request.ReturnUrl,
                        SignInResult = result
                    };
                }
                
                throw new DomainException("User and password not found");
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty();
                
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        
        public class AccountsController : MiruController
        {
            [HttpGet("/Accounts/Login")]
            public async Task<Command> Login(Query request) => await SendAsync(request);
            
            [HttpPost("/Accounts/Login")]
            public async Task<Result> Login(Command request) => await SendAsync(request);
        }
    }
}