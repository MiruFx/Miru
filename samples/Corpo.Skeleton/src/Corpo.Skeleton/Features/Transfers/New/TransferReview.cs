namespace Corpo.Skeleton.Features.Transfers.New;

public class TransferReview
{
    public class Query : IRequest<Command>
    {
    }
    
    public class Command : IRequest<FeatureResult>
    {
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
            await Task.CompletedTask;
            
            return new Command();
        }
            
        public async Task<FeatureResult> Handle(Command request, CancellationToken ct)
        {
            return new FeatureResult<TransferPay>();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
        }
    }
        
    public class TransfersController : MiruController
    {
        [HttpGet("/Transfers/New/Review")]
        public async Task<Command> Review(Query request) => await SendAsync(request);
        
        [HttpPost("/Transfers/New/Review")]
        public async Task<FeatureResult> Review(Command request) => await SendAsync(request);        
    }
}