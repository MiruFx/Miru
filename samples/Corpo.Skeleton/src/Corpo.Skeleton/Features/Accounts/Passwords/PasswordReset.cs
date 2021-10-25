using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Miru;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Userfy;

namespace Corpo.Skeleton.Features.Accounts.Passwords
{
    public class PasswordReset
    {
        public class Query : IRequest<Command>
        {
            public string Code { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
            public string Code { get; set; }
            public string Email { get; set; }
        }

        public class Result
        {
        }

        public class Handler : 
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Result>
        {
            private readonly IMailer _mailer;
            private readonly UserManager<User> _userManager;

            public Handler(IMailer mailer, UserManager<User> userManager)
            {
                _mailer = mailer;
                _userManager = userManager;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                return await Task.FromResult(new Command
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code))
                });
            }
            
            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                var user = await _userManager.FindByEmailAsync(command.Email);

                // if user does not exist don't tell the user
                if (user == null)
                {
                    App.Log.Warning("User with email {Email} not found", command.Email);
                    return new Result();
                }

                var result = await _userManager.ResetPasswordAsync(user, command.Code, command.Password);

                await _mailer.SendLaterAsync(new PasswordResetMail(user));
                
                if (result.Succeeded)
                {
                    return new Result();
                }

                throw result.Errors.ToDomainException();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Code).NotEmpty();
                
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                
                RuleFor(x => x.Password).NotEmpty()
                    .Equal(x => x.PasswordConfirmation)
                    .WithMessage("The password and confirmation password do not match");

                RuleFor(m => m.PasswordConfirmation).NotEmpty();
            }
        }
        
        public class PasswordController : MiruController
        {
            [HttpGet("/Accounts/Passwords/Reset")]
            public async Task<Command> Reset(Query query) => await SendAsync(query);

            [HttpPost("/Accounts/Passwords/Reset")]
            public async Task<Result> Reset(Command command) => await SendAsync(command);
        }
        
        public class PasswordResetMail : Mailable
        {
            private readonly User _user;

            public PasswordResetMail(User user)
            {
                _user = user;
            }

            public override void Build(Email mail)
            {
                mail.Subject("Reset Password")
                    .To(_user)
                    .Template("_Reset");
            }
        }
    }
}