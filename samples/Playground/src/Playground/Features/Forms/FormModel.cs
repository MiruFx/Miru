using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Html;
using Miru.Mvc;

namespace Playground.Features.Forms;

public class FormModel
{
    public class Query : IRequest<Command>
    {
    }

    public class Command : IRequest<Command>
    {
        // inputs
        [Display(Name = "Your Name")]
        public string Name { get; set; }
        
        [Radio]
        [Display(Name = "Are you local?")]
        public bool? IsLocal { get; set; }
        
        // lookups
    }

    public class Handler : 
        IRequestHandler<Query, Command>,
        IRequestHandler<Command, Command>
    {
        public Task<Command> Handle(Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Command() { IsLocal = true });
        }
            
        public Task<Command> Handle(Command request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
    }

    public class Validation : AbstractValidator<Command>
    {
        public Validation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class Controller : MiruController
    {
        [HttpGet("/Forms/Model")]
        public async Task<Command> Model(Query query) => await SendAsync(query);

        [HttpPost("/Forms/Model")]
        public async Task<Command> Model(Command command) => await SendAsync(command);
    }
}