using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Miru.Mailing;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Accounts.Passwords
{
    public class PasswordForgot
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Code { get; set; }
        }

        public class Result
        {
        }
        
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<User> _userManager;
            private readonly IMailer _mailer;

            public Handler(
                UserManager<User> userManager, 
                IMailer mailer)
            {
                _userManager = userManager;
                _mailer = mailer;
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                // dont reveal that user exist
                if (user == null)
                    return new Result();
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                request.Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                await _mailer.SendNowAsync(new PasswordForgotMail(user, request));
                
                return new Result();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
            }
        }
        
        public class PasswordsController : MiruController
        {
            [HttpGet("/Accounts/Passwords/Forgot")]
            public Command Forgot() => new();
            
            [HttpPost("/Accounts/Passwords/Forgot")]
            public async Task<Result> Forgot(Command request) => await SendAsync(request);
        }
        
        public class PasswordForgotMail : Mailable
        {
            private readonly User _user;
            private readonly Command _command;

            public PasswordForgotMail(User user, Command command)
            {
                _user = user;
                _command = command;
            }

            public override void Build(Email mail)
            {
                mail.To(_user.Email)
                    .Subject("Reset Password")
                    .Template("_Forgot", _command);
            }
        }
    }
}