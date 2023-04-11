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
            IRequestHandler<Command, FeatureResult>,
            IRequestHandler<Query, Command>
        {
            public Task<Command> Handle(Query request, CancellationToken ct)
            {
                return Task.FromResult(new Command());
            }

            public async Task<FeatureResult> Handle(Command request, CancellationToken ct)
            {
                return await Task.FromResult(new FeatureResult<ResultList>()
                    .Success("Success message")
                    .Alert("Alert message")
                    .Info("Info message")
                    .Danger("danger message"));
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