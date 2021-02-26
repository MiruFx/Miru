using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Userfy;
using Supportreon.Domain;

namespace Supportreon.Features.Accounts.Passwords
{
    public class PasswordEdit
    {
        public class Command : IRequest<Result>
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string NewPasswordConfirmation { get; set; }
        }

        public class Result
        {
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IMailer _mailer;
            private readonly UserManager<User> _userManager;
            private readonly IUserSession<User> _userSession;

            public Handler(IMailer mailer, UserManager<User> userManager, IUserSession<User> userSession)
            {
                _mailer = mailer;
                _userManager = userManager;
                _userSession = userSession;
            }

            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                var user = await _userManager.FindByIdAsync(_userSession.CurrentUserId.ToString());

                var result = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);

                if (result.Succeeded == false)
                    throw result.Errors.ToDomainException();

                await _mailer.SendLaterAsync(new PasswordEditMail(user));

                return new Result();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.CurrentPassword).NotEmpty();

                RuleFor(x => x.NewPassword).NotEmpty()
                    .Equal(x => x.NewPasswordConfirmation)
                    .WithMessage("The password and confirmation password do not match");
            }
        }
        
        public class PasswordController : MiruController
        {
            [HttpGet("/Accounts/Passwords/Edit")]
            public Command Edit() => new();

            [HttpPost("/Accounts/Passwords/Edit")]
            public async Task<Result> Edit(Command command) => await SendAsync(command);
        }
        
        public class PasswordEditMail : Mailable
        {
            private readonly User _user;

            public PasswordEditMail(User user)
            {
                _user = user;
            }

            public override void Build(Email mail)
            {
                mail.Subject("Password Changed")
                    .To(_user)
                    .Template("_Reset");
            }
        }
    }
}