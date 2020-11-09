using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Userfy;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Password
{
    public class PasswordForgot
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
        }

        public class Result
        {
        }
        
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly MongDbContext _db;
            private readonly IMailer _mailer;

            public Handler(MongDbContext db, IMailer mailer)
            {
                _db = db;
                _mailer = mailer;
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var user =  await _db.Users.SingleOrFailAsync(x => x.Email == request.Email, $"Could not find email {request.Email}", ct);

                user.RequestedPasswordReset();

                await _mailer.SendLater(new PasswordForgotMail(user));

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
        
        public class PasswordForgotMail : Mailable
        {
            private readonly User _user;

            public PasswordForgotMail(User user) => _user = user;

            public override void Build(Email mail)
            {
                mail.To(_user.Email)
                    .Subject("Reset Password")
                    .Template(_user);
            }
        }
        
        public class PasswordController : MiruController
        {
            public Command Forgot() => new Command();

            [HttpPost]
            public async Task<Result> Forgot(Command command) => await Send(command);
        }
    }
}