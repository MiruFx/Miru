namespace Corpo.Skeleton.Features.Teams;

public class TeamEdit
{
    public class Query : IRequest<Command>
    {
        public long Id { get; set; }
    }
        
    public class Command : IRequest<FeatureResult>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Handler : 
        IRequestHandler<Query, Command>, 
        IRequestHandler<Command, FeatureResult>
    {
        private readonly SkeletonDbContext _db;
            
        public Handler(SkeletonDbContext db)
        {
            _db = db;
        }
            
        public async Task<Command> Handle(Query request, CancellationToken ct)
        {
            var team = await _db.Teams.ByIdOrNewAsync(request.Id, ct);
                
            return new Command
            {
                Id = team.Id,
                Name = team.Name
            };
        }
            
        public async Task<FeatureResult> Handle(Command request, CancellationToken ct)
        {
            var team = await _db.Teams.ByIdOrNewAsync(request.Id, ct);
                
            team.Name = request.Name;

            await _db.AddOrUpdateAsync(team, ct);

            return new FeatureResult<TeamList>();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
        
    public class TeamsController : MiruController
    {
        [HttpGet("/Teams/New")]
        public async Task<Command> Edit(TeamNew query) => await SendAsync(new Query());
            
        [HttpGet("/Teams/{id:long}/Edit")]
        public async Task<Command> Edit(Query query) => await SendAsync(query);

        [HttpPost("/Teams/Edit")]
        public async Task<FeatureResult> Edit(Command command) => await SendAsync(command);
    }
}