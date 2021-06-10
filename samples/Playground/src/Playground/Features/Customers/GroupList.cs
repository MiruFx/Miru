using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Mvc;

namespace Playground.Features.Customers
{
    public class CustomerList
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Item> Customers = new();
        }
        
        public class Item : IHasId
        {
            public long Id => CustomerId;
            public long CustomerId { get; set; }
            public string CustomerName { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return await Task.FromResult(new Result
                {
                    Customers = new()
                    {
                        new() { CustomerName = "John Lennon"},
                        new() { CustomerName = "Paul McCartney"}
                    }
                });
            }
        }

        public class CustomerController : MiruController
        {
            [HttpGet("/Customers")]
            public async Task<Result> List(Query request) => await SendAsync(request);
        }
    }
}
