using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru;
using Miru.Mvc;

namespace Playground.Features.Results
{
    public class ResultEdit
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : IRequest<Feature>
        {
        }

        public class Handler :
            RequestHandler<Command, Feature>,
            IRequestHandler<Query, Command>
        {
            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command());
            }

            protected override Feature Handle(Command request)
            {
                return new Feature<ResultList>();
            }
        }

        public class ResultsController : MiruController
        {
            [HttpGet("/Results/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost("/Results/Edit")]
            public async Task<Feature> Edit(Command command) => await SendAsync(command);
        }
    }
}