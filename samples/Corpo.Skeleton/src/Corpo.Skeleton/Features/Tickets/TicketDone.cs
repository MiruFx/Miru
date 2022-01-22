namespace Corpo.Skeleton.Features.Tickets;

public class TicketDone
{
    public class Query : IRequest<Result>
    {
    }

    public class Result
    {    
        public IReadOnlyList<TicketView> Tickets { get; set; } = new List<TicketView>();
    }

    public class TicketView
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly AppDbContext _db;
            
        public Handler(AppDbContext db)
        {
            _db = db;
        }
            
        public async Task<Result> Handle(Query request, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            return new Result
            {
                Tickets = new List<TicketView>
                {
                    new() { Id = 1, Name = "Remove virus from Accounting computer" },
                    new() { Id = 2, Name = "Change CEO's keyboard" },
                }
            };
        }
    }
        
    public class TicketsController : MiruController
    {
        [HttpGet("/Tickets")]
        public async Task<Result> List(Query request) => await SendAsync(request);
    }
}