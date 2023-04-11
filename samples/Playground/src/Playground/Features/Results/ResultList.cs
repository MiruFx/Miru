using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;

namespace Playground.Features.Results
{
    public class ResultList
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
        }

        public class Handler :
            IRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return await Task.FromResult(new Result());
            }
        }

        public class ResultsController : MiruController
        {
            [HttpGet("/Results")]
            public async Task<Result> List(Query query) => await SendAsync(query);
        }
    }
}