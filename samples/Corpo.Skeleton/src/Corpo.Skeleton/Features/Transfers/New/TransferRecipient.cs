namespace Corpo.Skeleton.Features.Transfers.New;

public class TransferRecipient
{
    public class Query : IRequest<Command>
    {
    }
    
    public class Command : IRequest<FeatureResult>
    {
        public TransferNew.Command Amount { get; set; }
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
            return new FeatureResult<TransferReview>();
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
        [HttpGet("/Transfers/New/Recipient")]
        public async Task<Command> Recipient(Query request) => await SendAsync(request);
        
        [HttpPost("/Transfers/New/Recipient")]
        public async Task<FeatureResult> Recipient(Command request) => await SendAsync(request);        
    }
}