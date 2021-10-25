namespace Corpo.Skeleton.Features.Categories;

public class CategoryNew
{
    public class Query : IRequest<Command>
    {
    }
        
    public class Command : IRequest<Result>
    {
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
            
        public Task<Command> Handle(Query request, CancellationToken ct)
        {
            return Task.FromResult(new Command());
        }
            
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var category = new Category
            {
                Name = request.Name
            };

            await _db.Categories.AddAsync(category, ct);

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
        
    public class CategoriesController : MiruController
    {
        [Route("/Categories/New")]
        public async Task<Command> New(Query query) => await SendAsync(query);

        [HttpPost, Route("/Categories/New")]
        public async Task<Result> New(Command command) => await SendAsync(command);
    }
}