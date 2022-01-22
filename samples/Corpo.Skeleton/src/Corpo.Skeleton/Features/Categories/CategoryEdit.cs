namespace Corpo.Skeleton.Features.Categories;

public class CategoryEdit
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
        private readonly AppDbContext _db;
            
        public Handler(AppDbContext db)
        {
            _db = db;
        }
            
        public async Task<Command> Handle(Query request, CancellationToken ct)
        {
            var category = await _db.Categories.ByIdOrFailAsync(request.Id, ct);
                
            return new Command
            {
                Id = category.Id,
                Name = category.Name
            };
        }
            
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var category = await _db.Categories.ByIdOrFailAsync(request.Id, ct);

            category.Name = request.Name;
                
            return new Result();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
                
            RuleFor(m => m.Name).NotEmpty();
        }
    }
        
    public class CategoriesController : MiruController
    {
        [Route("/Categories/{id:long}/Edit")]
        public async Task<Command> Edit(Query query) => await SendAsync(query);

        [HttpPost, Route("/Categories/Edit")]
        public async Task<Result> Edit(Command command) => await SendAsync(command);
    }
}