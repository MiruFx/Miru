using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Userfy;
using Mong.Database;

namespace Mong.Features.Password
{
    public class PasswordReset
    {
        public class Query : IRequest<Command>
        {
            public string Token { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public string Token { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }

        public class Result
        {
        }

        public class Handler : 
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Result>
        {
            private readonly MongDbContext _db;
            private readonly IMailer _mailer;
            private readonly UserfyOptions _userfyOptions;

            public Handler(MongDbContext db, IMailer mailer, UserfyOptions userfyOptions)
            {
                _db = db;
                _mailer = mailer;
                _userfyOptions = userfyOptions;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var user = await _db.Users
                    .Where(m => m.ResetPasswordToken == request.Token)
                    .SingleOrFailAsync("Could not find token ", ct);

                // user.EnsureTokenIsValid(_userfyOptions.ResetPasswordWithin);
                
                return new Command { Token = request.Token };
            }
            
            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                var user =  await _db.Users
                    .Where(m => m.ResetPasswordToken == command.Token)
                    .SingleOrFailAsync($"Could not find token {command.Token}", ct);

                // user.EnsureTokenIsValid(_userfyOptions.ResetPasswordWithin);
                //
                // user.ResetPassword(Hash.Create(command.Password));
                //
                // await _mailer.SendLaterAsync(user, new PasswordResetMail());
                
                return new Result();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Token).NotEmpty();
                
                RuleFor(x => x.Password).NotEmpty().Equal(x => x.PasswordConfirmation).WithMessage("The password and confirmation password do not match");

                RuleFor(m => m.PasswordConfirmation).NotEmpty();
            }
        }
        
        public class PasswordController : MiruController
        {
            public async Task<Command> Reset(Query query) => await SendAsync(query);

            [HttpPost]
            public async Task<Result> Reset(Command command) => await SendAsync(command);
        }
        
        public class PasswordResetMail : Mailable
        {
            public override void Build(Email mail)
            {
                mail.Subject("Reset Password")
                    .Template(string.Empty);
            }
        }
    }
}