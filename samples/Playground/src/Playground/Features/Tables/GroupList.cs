using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Mvc;
using Playground.Database;

namespace Playground.Features.Tables
{
    public class TableList
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Product> Products = new();
        }
        
        public class Product : IHasId
        {
            public long Id => ProductId;
            public long ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
            public DateTime? ProductLastSale { get; set; }
            public bool ProductIsActive { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PlaygroundFabricator _fab;

            public Handler(PlaygroundFabricator fab)
            {
                _fab = fab;
            }

            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return await Task.FromResult(new Result
                {
                    Products = _fab.MakeMany<Product>(30).ToList()
                });
            }
        }

        public class TablesController : MiruController
        {
            [HttpGet("/Tables")]
            public async Task<Result> List(Query request) => await SendAsync(request);
        }
    }
}
