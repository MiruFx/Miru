namespace Corpo.Skeleton.Features.Teams;

public class TeamList
{
    public class Query : IRequest<Result>
    {
    }

    public class Result
    {    
        public IReadOnlyList<TeamView> Teams { get; set; } = new List<TeamView>();
    }

    public class TeamView
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
            return new Result
            {
                Teams = new List<TeamView>()
                // Teams = await _db.Teams
                //     .Select(m => new TeamView
                //     {
                //         Id = m.Id,
                //         Name = m.Name
                //     })
                //     .ToListAsync(ct)
            };
        }
    }
        
    public class Controller : MiruController
    {
        [HttpGet("/Teams")]
        public async Task<Result> List(Query request) => await SendAsync(request);
    }
}