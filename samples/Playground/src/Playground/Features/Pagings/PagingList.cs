using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Domain;
using Miru.Mvc;
using Miru.Pagination;
using Playground.Database;

namespace Playground.Features.Pagings
{
    public class PagingList
    {
        public class Query : 
            // the query returns itself
            IRequest<Query>, 
            // the Query has to implements IPageable of the item's type
            // in this case is pageable of products
            IPageable<Product>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Pages { get; set; }
            public int CountShowing { get; set; }
            public int CountTotal { get; set; }
            
            // the paginated results should be assigned in this property
            public IReadOnlyList<Product> Results { get; set; }
        }

        public class Product : IHasId
        {
            public long Id => ProductId;
            public long ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
        }

        public class Handler : 
            // the Query returns itself to keep the pagination counters' state:
            // which page it was, page size the user requested, and etc
            IRequestHandler<Query, Query>
        {
            private readonly PlaygroundFabricator _fab;

            public Handler(PlaygroundFabricator fab)
            {
                _fab = fab;
            }

            public async Task<Query> Handle(Query request, CancellationToken ct)
            {
                // the Handle should use the same instance to update the pagination state
                request.PageSize = 10;
                
                request.Results = await Task.FromResult(_fab
                    .MakeMany<Product>(200, p =>
                    {
                        p.ProductName = _fab.Faker.Commerce.ProductName();
                    })
                    .ToPaginate(request));
                
                return request;
            }
        }

        public class PagingsController : MiruController
        {
            [HttpGet("/Pagings")]
            public async Task<Query> List(Query request) => await SendAsync(request);
        }
    }
}