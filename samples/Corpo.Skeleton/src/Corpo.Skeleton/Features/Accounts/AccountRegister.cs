﻿using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Database;
using Corpo.Skeleton.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru;
using Miru.Domain;
using Miru.Mailing;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Accounts
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
            private readonly SkeletonDbContext _db;
            private readonly IMailer _mailer;

            public Handler(SkeletonDbContext db, IMailer mailer)
            {
                _db = db;
                _mailer = mailer;
            }

            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                if (await _db.Users.AnyAsync(x => x.Email == command.Email, ct))
                    throw new DomainException("Email is already in use. It should be unique");
                
                var user = new User
                {
                    Name = command.Name,
                    Email = command.Email,
                    HashedPassword = Hash.Create(command.Password)
                };

                await _db.Users.AddAsync(user, ct);

                await _mailer.SendLaterAsync(new AccountRegisteredMail(user));
                
                return new Result
                {
                    User = user
                };
            }
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