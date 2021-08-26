using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Domain;
using Miru.Mvc;
using Miru.Pagination;
using Playground.Database;

namespace Playground.Features.Enumerations
{
    public class EnumerationList
    {
        public class OrderStatus : Enumeration<OrderStatus>
        {
            public static OrderStatus Created = new(1, "Created");
            public static OrderStatus PendingPayment = new(2, "Pending Payment");
            public static OrderStatus InPreparation = new(3, "In Preparation");
            public static OrderStatus Shipped = new(4, "Shipped");
            public static OrderStatus Received = new(5, "Received");
            
            public OrderStatus(int value, string name) : base(value, name)
            {
            }
        }
        
        public class PaymentStatus : Enumeration<PaymentStatus, string>
        {
            public static PaymentStatus Pending = new("PE", "Pending");
            public static PaymentStatus Paid = new("PA", "Pending Payment");
            public static PaymentStatus Refused = new("NO", "Refused");
            public static PaymentStatus Refund = new("RE", "Refund");

            public PaymentStatus(string value, string name) : base(value, name)
            {
            }
        }
        
        public class Query : IRequest<Query>
        {
            public OrderStatus OrderStatus { get; set; }
            public PaymentStatus PaymentStatus { get; set; }

            public IReadOnlyList<Order> Results { get; set; } = new List<Order>();
        }
        
        public class Order : IHasId
        {
            public long Id { get; set; }
            public OrderStatus OrderStatus { get; set; }
            public PaymentStatus PaymentStatus { get; set; }
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
                var query = _fab.MakeMany<Order>(200, p =>
                {
                    p.Id = _fab.Faker.Random.Int(1, 200);
                    p.OrderStatus = _fab.Faker.PickRandom(OrderStatus.GetAll());
                    p.PaymentStatus = _fab.Faker.PickRandom(PaymentStatus.GetAll());
                });

                if (request.OrderStatus != null)
                    query = query.Where(x => x.OrderStatus == request.OrderStatus);
                
                if (request.PaymentStatus != null)
                    query = query.Where(x => x.PaymentStatus == request.PaymentStatus);
                
                request.Results = query.ToList();
                
                return await Task.FromResult(request);
            }
        }

        public class PagingsController : MiruController
        {
            [HttpGet("/Enumerations")]
            public async Task<Query> List(Query request) => await SendAsync(request);
        }
    }
}