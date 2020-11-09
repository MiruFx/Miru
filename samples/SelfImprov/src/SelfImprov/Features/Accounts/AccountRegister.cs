using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mailing;
using Miru.Mvc;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Features.Accounts
{
    public class AccountRegister
    {
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public string Email { get; set; }  
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }

        public class Result
        {
            public User User { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly SelfImprovDbContext _db;
            private readonly IMailer _mailer;

            public Handler(SelfImprovDbContext db, IMailer mailer)
            {
                _db = db;
                _mailer = mailer;
            }

            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                var user = new User
                {
                    Name = command.Name,
                    Email = command.Email,
                    HashedPassword = Hash.Create(command.Password)
                };

                await _db.Users.AddAsync(user, ct);

                await _mailer.SendLater(new AccountRegisteredMail(user));
                
                return new Result
                {
                    User = user
                };
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(SelfImprovDbContext db)
            {
                RuleFor(x => x.Name).NotEmpty();
                
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MustAsync(async (s, ct) => await db.Users.NoneAsync(x => x.Email == s, ct))
                        .WithMessage("Email is already in use. It should be unique");

                RuleFor(x => x.Password).NotEmpty().Equal(x => x.PasswordConfirmation).WithMessage("The password and confirmation password do not match");
                
                RuleFor(x => x.PasswordConfirmation).NotEmpty();
            }
        }

        public class AccountsController : MiruController
        {
            public Command Register() => new Command();

            [HttpPost]
            public async Task<Result> Register(Command command) => await Send(command);
        }
    }
}
