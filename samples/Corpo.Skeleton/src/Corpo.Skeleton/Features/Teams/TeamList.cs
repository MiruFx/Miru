namespace Corpo.Skeleton.Features.Teams;

public class TeamList
{
    public class Query : IRequest<Result>
    {
    }

    public class Result
    {    
        public IReadOnlyList<Item> Teams { get; set; } = new List<Item>();
    }

    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly SkeletonDbContext _db;
            
        public Handler(SkeletonDbContext db)
        {
            _db = db;
        }
            
        public async Task<Result> Handle(Query request, CancellationToken ct)
        {
            return new Result
            {
                Teams = await _db.Teams
                    .Select(m => new Item
                    {
                        Id = m.Id,
                        Name = m.Name
                    })
                    .ToListAsync(ct)
            };
        }
    }
        
    public class Controller : MiruController
    {
        [HttpGet("/Teams")]
        public async Task<Result> List(Query request) => await SendAsync(request);
    }
}