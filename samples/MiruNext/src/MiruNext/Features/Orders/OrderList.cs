using MiruNext.Domain;

namespace MiruNext.Features.Orders;

public class OrderList
{
    public class Query
    {
        public int Page { get; set; }
        
        public OrderStatuses Status { get; set; }
    }

    public class Result : Query
    {
        public Order[] Orders { get; set; } = Array.Empty<Order>();
    }

    [HttpGet("/Orders")]
    public class List : Endpoint2<Query>
    {
        public override async Task HandleAsync(Query request, CancellationToken ct)
        {
            var result = new Result
            {
                Orders = new[]
                {
                    new Order { Id = 1, CreatedAt = 1.OfPreviousMonth() }, 
                    new Order { Id = 2, CreatedAt = 5.OfPreviousMonth() },
                    new Order { Id = 3, CreatedAt = 5.OfPreviousMonth() },
                    new Order { Id = 4, CreatedAt = 5.OfPreviousMonth() },
                    new Order { Id = 5, CreatedAt = 5.OfPreviousMonth() },
                    new Order { Id = 6, CreatedAt = 5.OfPreviousMonth() },
                    new Order { Id = 8, CreatedAt = 5.OfPreviousMonth() },
                    new Order { Id = 9, CreatedAt = 5.OfPreviousMonth() },
                },
                Page = request.Page,
                Status = request.Status
            };

            await RespondAsync(result);
        }
    }
}