namespace Corpo.Skeleton.Features.Tickets;

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
            await Task.CompletedTask;
            
            return new Command();
        }
            
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            await Task.CompletedTask;

            return new Result();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty();
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