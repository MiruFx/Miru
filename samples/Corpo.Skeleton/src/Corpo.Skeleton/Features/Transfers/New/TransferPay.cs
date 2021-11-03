namespace Corpo.Skeleton.Features.Transfers.New;

public class TransferPay
{
    public class Query : IRequest<Command>
    {
    }
    
    public class Command : IRequest<Result>
    {
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
            
        public async Task<Command> Handle(Query request, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            return new Command();
        }
            
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            return new Result();
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
        [HttpGet("/Transfers/New/Pay")]
        public async Task<Command> Pay(Query request) => await SendAsync(request);
        
        [HttpPost("/Transfers/New/Pay")]
        public async Task<Result> Pay(Command request) => await SendAsync(request);        
    }
}