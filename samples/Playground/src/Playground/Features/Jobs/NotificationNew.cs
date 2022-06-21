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

public class NotificationNew
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
            _jobs.Enqueue(new NameNewEvent { Name = request.Name });
            
            return Task.FromResult(new FeatureResult(new Command()));
        }
    }

    public class Controller : MiruController
    {
        [HttpGet("/Notifications")]
        public async Task<Command> Notification(Query query) => await SendAsync(query);

        [HttpPost("/Notifications")]
        public async Task<FeatureResult> Notification(Command command) => await SendAsync(command);
    }
    
    public class NameNewEvent : INotification
    {   
        public string Name { get; set; }
    }
    
    public class NameNewJob1 : IRequest<NameNewJob1>
    {   
        public string Name { get; set; }
    }
    
    public class NameNewJob2 : IRequest<NameNewJob2>
    {   
        public string Name { get; set; }
    }
    
    public class JobHandler1 : 
        IRequestHandler<NameNewJob1, NameNewJob1>,
        INotificationHandler<NameNewEvent>
    {
        private readonly PlaygroundDbContext _db;
        private readonly IMiruApp _app;

        public JobHandler1(PlaygroundDbContext db, IMiruApp app)
        {
            _db = db;
            _app = app;
        }
        
        public async Task<NameNewJob1> Handle(NameNewJob1 request, CancellationToken ct)
        {
            App.Log.Information("DbContext hashcode {HashCode}", _db.GetHashCode());
            
            App.Log.Information("Name from JobHandler1 {Name}", request.Name);

            return await Task.FromResult(request);
        }

        public Task Handle(NameNewEvent notification, CancellationToken ct) =>
            _app.EnqueueAsync(new NameNewJob1 { Name = notification.Name });
    }
    
    public class JobHandler2 : 
        IRequestHandler<NameNewJob2, NameNewJob2>,
        INotificationHandler<NameNewEvent>
    {
        private readonly PlaygroundDbContext _db;
        private readonly IMiruApp _app;

        public JobHandler2(PlaygroundDbContext db, IMiruApp app)
        {
            _db = db;
            _app = app;
        }
        
        public async Task<NameNewJob2> Handle(NameNewJob2 request, CancellationToken ct)
        {
            App.Log.Information("DbContext hashcode {HashCode}", _db.GetHashCode());
            
            App.Log.Information("Name from JobHandler2 {Name}", request.Name);

            return await Task.FromResult(request);
        }

        public Task Handle(NameNewEvent notification, CancellationToken ct) =>
            _app.EnqueueAsync(new NameNewJob2 { Name = notification.Name });
    }
}