namespace Corpo.Skeleton.Features.Categories;

public class CategoryShow
{
    public class Query : IRequest<Result>
    {
        public long Id { get; set; }
    }

    public class Result
    {
        public Category Category { get; set; }
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
                Category = await _db.Categories.ByIdOrFailAsync(request.Id, ct)
            };
        }
    }
        
    public class CategoriesController : MiruController
    {
        [Route("/Categories/{id:long}")]
        public async Task<Result> Show(Query request) => await SendAsync(request);
    }
}