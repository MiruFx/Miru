namespace Corpo.Skeleton.Features.Transfers.New;

public class TransferNew
{
    public class Query : IRequest<Command>
    {
    }
    
    public class Command : IRequest<Result>
    {
        public decimal AmountSource { get; set; }
        public string CurrencySource { get; set; }
        public string CurrencyRecipient { get; set; }
        
        // currencies
        public Lookups Currencies { get; set; } = new Dictionary<string, string>()
        {
            { "EUR", "EUR" },
            { "USD", "USD" },
            { "BRL", "BRL" },
        }.ToLookups();
    }

    public class Result
    {
        public Command Amount { get; set; }
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
            
            return new Result
            {
                Amount = request
            };
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AmountSource).GreaterThan(0);
        }
    }
        
    public class TransfersController : MiruController
    {
        [HttpGet("/Transfers/New")]
        public async Task<Command> New(Query request) => await SendAsync(request);
        
        [HttpPost("/Transfers/New")]
        public async Task<Result> New(Command request) => await SendAsync(request);        
    }
}