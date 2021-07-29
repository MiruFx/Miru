using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Mvc;

namespace Playground.Features.Results
{
    public class PartialShow
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result : IPartialResult
        {
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Result());
            }
        }

        public class ResultsController : MiruController
        {
            [HttpGet("/Results/Partial")]
            public async Task<Result> Partial(Query query) => await SendAsync(query);
        }
    }
}