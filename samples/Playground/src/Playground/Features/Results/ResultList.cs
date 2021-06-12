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
            RequestHandler<Query, Result>
        {
            protected override Result Handle(Query request)
            {
                return new Result();
            }
        }

        public class ResultsController : MiruController
        {
            [HttpGet("/Results")]
            public async Task<Result> List(Query query) => await SendAsync(query);
        }
    }
}