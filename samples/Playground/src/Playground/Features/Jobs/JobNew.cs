using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Miru;
using Miru.Mvc;
using Playground.Database;

namespace Playground.Features.Jobs;

public class JobNew
{
    public class Query : IRequest<Command>
    {
    }

    public class Command : IRequest<FeatureResult>
    {   
        [Display(Name = "Your Name")]
        public string Name { get; set; }
    }

    public class Handler : 
        IRequestHandler<Query, Command>,
        IRequestHandler<Command, FeatureResult>
    {
        private readonly Miru.Queuing.Jobs _jobs;

        public Handler(Miru.Queuing.Jobs jobs)
        {
            _jobs = jobs;
        }

        public Task<Command> Handle(Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Command());
        }
            
        public Task<FeatureResult> Handle(Command request, CancellationToken cancellationToken)
        {
            _jobs.Enqueue(new NameNewJob { Name = request.Name });
            
            return Task.FromResult(new FeatureResult(new Command()));
        }
    }

    public class Controller : MiruController
    {
        [HttpGet("/Jobs")]
        public async Task<Command> New(Query query) => await SendAsync(query);

        [HttpPost("/Jobs")]
        public async Task<FeatureResult> New(Command command) => await SendAsync(command);
    }
    
    public class NameNewJob : IRequest
    {   
        public string Name { get; set; }
    }
    
    public class JobHandler : IRequestHandler<NameNewJob>
    {
        private readonly ILogger<JobHandler> _logger;
        private readonly PlaygroundDbContext _db;

        public JobHandler(ILogger<JobHandler> logger, PlaygroundDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<Unit> Handle(NameNewJob request, CancellationToken cancellationToken)
        {
            _db.Users.ToList();
            
            _logger.LogInformation("From ILogger {Name}", request.Name);
            
            App.Log.Information("Name is {Name}", request.Name);

            return await Unit.Task;
        }
    }
}