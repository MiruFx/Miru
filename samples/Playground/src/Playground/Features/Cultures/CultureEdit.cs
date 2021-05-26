using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Html;
using Miru.Mvc;

namespace Playground.Features.Cultures
{
    public class CultureForm
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : IRequest<Command>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Handler :
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Command>
        {
            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command());
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
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
            }
        }

        public class Controller : MiruController
        {
            [HttpGet("/Cultures/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost("/Cultures/Edit")]
            public async Task<Command> Edit(Command command) => await SendAsync(command);
        }
    }
}