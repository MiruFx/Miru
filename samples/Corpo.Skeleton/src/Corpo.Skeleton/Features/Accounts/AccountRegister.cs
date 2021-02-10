using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Miru;
using Miru.Domain;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Userfy;

namespace Corpo.Skeleton.Features.Accounts
{
    public class AccountRegister
    {
        public class Query : IRequest<Command>
        {
            public string ReturnUrl { get; set; }
        }
        
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
            
            public string ReturnUrl { get; set; }
        }

        public class Result : IRedirect
        {
            public IdentityResult IdentityResult { get; set; }
            public object RedirectTo { get; set; }
        }
        
        public class Handler :
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Result>
        {
            private readonly IUserStore<User> _userStore;
            private readonly IUserEmailStore<User> _emailStore;
            private readonly UserManager<User> _userManager;
            private readonly IMailer _mailer;
            private readonly IUserSession _userSession;

            public Handler(
                SignInManager<User> signInManager, 
                IUserStore<User> userStore, 
                UserManager<User> userManager, 
                IMailer mailer, 
                IUserSession userSession)
            {
                _userStore = userStore;
                _emailStore = (IUserEmailStore<User>)_userStore;
                _userManager = userManager;
                _mailer = mailer;
                _userSession = userSession;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                return await Task.FromResult(new Command
                {
                    ReturnUrl = request.ReturnUrl
                });
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                request.ReturnUrl ??= "/";

                var user = new User();

                await _userStore.SetUserNameAsync(user, request.Email, ct);
                await _emailStore.SetEmailAsync(user, request.Email, ct);
                
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _mailer.SendLaterAsync(new AccountRegisteredMail(user));

                    await _userSession.LoginAsync(request.Email, request.Password);
                }
                else
                {
                    throw new DomainException(result.Errors.Select(x => x.Description).Join(". "));
                }

                return new Result
                {
                    RedirectTo = request.ReturnUrl,
                    IdentityResult = result
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();

                RuleFor(x => x.Password).NotEmpty()
                    .Equal(x => x.PasswordConfirmation)
                    .WithMessage("The password and confirmation password do not match");
                
                RuleFor(x => x.PasswordConfirmation).NotEmpty();
            }
        }
        
        public class AccountsController : MiruController
        {
            [HttpGet("/Accounts/Register")]
            public async Task<Command> Register(Query request) => await SendAsync(request);
            
            [HttpPost("/Accounts/Register")]
            public async Task<Result> Register(Command request) => await SendAsync(request);
        }
    }
}