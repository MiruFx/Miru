using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;
using Mong.Database;
using Mong.Domain;

namespace Mong.Features.Admin.Users
{
    public class UserBlockUnblock
    {
        public class Command : IRequest<Result>
        {
            public long UserId { get; set; }
        }
        
        public class Result
        {
            public User User { get; set; }
        }
        
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly MongDbContext _db;

            public Handler(MongDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var user = await _db.Users.ByIdAsync(request.UserId, ct);

                user.BlockOrUnblock();

                return new Result { User = user };
            }
        }

        [Route("Admin/Users/{UserId:long}")]
        public class UsersController : MiruController
        {
            [HttpPost]
            public async Task<Result> BlockUnblock(Command request) => await SendAsync(request);
        }
    }
}