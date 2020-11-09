using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Behaviors;
using Miru.Domain;
using Miru.Mvc;
using Miru.Userfy;
using Skeleton.Database;

namespace Skeleton.Features.Accounts
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
            public bool Remember { get; set; }
            public string ReturnUrl { get; set; }
        }

        public class Result : IRedirectResult
        {
            public string RedirectTo { get; set; }
        }
        
        public class Handler : RequestHandler<Query, Command>, IRequestHandler<Command, Result>
        {
            private readonly SkeletonDbContext _db;
            private readonly IUserSession _userSession;

            public Handler(SkeletonDbContext db, IUserSession userSession)
            {
                _db = db;
                _userSession = userSession;
            }

            protected override Command Handle(Query request)
            {
                return new Command { ReturnUrl = request.ReturnUrl };
            }
            
            public async Task<Result> Handle(Command command, CancellationToken ct)
            {
                var user = await _db.Users
                    .Where(_ => _.Email == command.Email && _.HashedPassword == Hash.Create(command.Password))
                    .SingleOrFailAsync("User and password not found", ct);

                _userSession.Login(user, command.Remember);

                return new Result
                {
                    RedirectTo = command.ReturnUrl.Or("/")
                };
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
            public async Task<Command> Login(Query query) => await Send(query);

            [HttpPost]
            public async Task<Result> Login(Command command) => await Send(command);
        }
    }
}