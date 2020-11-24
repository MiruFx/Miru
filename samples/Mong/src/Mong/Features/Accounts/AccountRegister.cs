using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Domain;
using Miru.Html;
using Miru.Mailing;
using Miru.Mvc;
using Mong.Config;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Accounts
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
            private readonly MongDbContext _db;
            private readonly IMailer _mailer;

            public Handler(MongDbContext db, IMailer mailer)
            {
                _db = db;
                _mailer = mailer;
            }

            // #handle
            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                if (await _db.Users.AnyAsync(x => x.Email == command.Email, ct))
                    throw new DomainException("Email is already in use. It should be unique");
                
                var user = new User
                {
                    Name = command.Name,
                    Email = command.Email,
                    HashedPassword = Hash.Create(command.Password),
                    ConfirmationToken = Guid.NewGuid().ToString(),
                    ConfirmationSentAt = DateTime.Now
                };

                _db.Users.Add(user);

                await _mailer.SendLaterAsync(new AccountRegisteredMail(user));
                
                return new Result
                {
                    User = user
                };
            }
            // #handle
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();

                RuleFor(x => x.Email).NotEmpty().EmailAddress();

                RuleFor(x => x.Password).NotEmpty().Equal(x => x.PasswordConfirmation).WithMessage("The password and confirmation password do not match");
                
                RuleFor(x => x.PasswordConfirmation).NotEmpty();
            }
        }

        public class AccountsController : MiruController
        {
            public Command Register() => new Command();

            [HttpPost]
            public async Task<Result> Register(Command command) => await SendAsync(command);
        }
    }
}