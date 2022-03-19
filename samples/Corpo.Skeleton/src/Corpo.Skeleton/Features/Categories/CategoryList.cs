namespace Corpo.Skeleton.Features.Categories;

public class CategoryList
{
    public class Query : IRequest<Result>
    {
    }

    public class Result
    {    
        public IReadOnlyList<Item> Items { get; set; } = new List<Item>();
    }

    public class Item
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
                Items = await _db.Categories
                    .Select(m => new Item
                    {
                        Id = m.Id,
                        Name = m.Name
                    })
                    .ToListAsync(ct)
            };
        }
    }
        
    public class CategoriesController : MiruController
    {
        [HttpGet("/Categories")]
        public async Task<Result> List(Query request) => await SendAsync(request);
    }
}