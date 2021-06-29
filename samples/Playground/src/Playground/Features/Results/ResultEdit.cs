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

        public class Command : IRequest<FeatureResult>
        {
        }

        public class Handler :
            RequestHandler<Command, FeatureResult>,
            IRequestHandler<Query, Command>
        {
            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command());
            }

            protected override FeatureResult Handle(Command request)
            {
                return new FeatureResult<ResultList>();
            }
        }

        public class ResultsController : MiruController
        {
            [HttpGet("/Results/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost("/Results/Edit")]
            public async Task<FeatureResult> Edit(Command command) => await SendAsync(command);
        }
    }
}