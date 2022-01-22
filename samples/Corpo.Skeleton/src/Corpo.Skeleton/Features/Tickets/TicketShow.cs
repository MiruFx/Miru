namespace Corpo.Skeleton.Features.Tickets;

public class TicketShow
{
    public class Query : IRequest<Result>
    {
        public long Id { get; set; }
    }

    public class Result
    {
        public string Name { get; set; }
    }

    public class Handler : IRequestEmptyHandler<Query, Result>
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
                Name = "Change CEO's keyboard"
            };
        }
    }
        
    public class TicketsController : MiruController
    {
        [HttpGet("/Tickets/{id:long}")]
        public async Task<Result> Show(Query request) => await SendAsync(request);
    }
}