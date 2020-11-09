using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;
using Miru.Security;
using Miru.Userfy;
using Mong.Domain;

namespace Mong.Features.Password
{
    public class PasswordEdit : IMustBeAuthenticated
    {
        public class Command : IRequest
        {
            public string CurrentPassword { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUserSession<User> _currentUser;

            public Handler(IUserSession<User> currentUser)
            {
                _currentUser = currentUser;
            }

            public async Task<Unit> Handle(Command request, CancellationToken ct)
            {
                var user = await _currentUser.User();

                user.ChangePassword(request);
                
                return Unit.Value;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPassword).NotEmpty();
                
                RuleFor(x => x.Password).NotEmpty().Equal(x => x.PasswordConfirmation).WithMessage("The password and confirmation password do not match");
                
                RuleFor(x => x.PasswordConfirmation).NotEmpty();
            }
        }
        
        public class PasswordController : MiruController
        {
            public Command Edit() => new Command();

            [HttpPost]
            public async Task<Unit> Edit(Command command) => await Send(command);
        }
    }
}