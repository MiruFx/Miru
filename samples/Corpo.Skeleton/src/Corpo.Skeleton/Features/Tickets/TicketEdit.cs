using System.Threading;
using System.Threading.Tasks;
using Corpo.Skeleton.Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;

namespace Corpo.Skeleton.Features.Tickets
{
    public class TicketEdit
    {
        public class Query : IRequest<Command>
        {
            public long Id { get; set; }
        }
        
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class Result
        {
        }

        public class Handler : 
            IRequestHandler<Query, Command>, 
            IRequestHandler<Command, Result>
        {
            private readonly SkeletonDbContext _db;
            
            public Handler(SkeletonDbContext db)
            {
                _db = db;
            }
            
            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                return await Task.FromResult(new Command());
            }
            
            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                return await Task.FromResult(new Result());
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotEmpty();
            }
        }
        
        public class Controller : MiruController
        {
            [HttpGet("/Tickets/{id:long}/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost("/Tickets/Edit")]
            public async Task<Result> Edit(Command command) => await SendAsync(command);
        }
    }
}