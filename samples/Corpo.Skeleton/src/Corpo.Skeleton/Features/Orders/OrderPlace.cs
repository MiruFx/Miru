namespace Corpo.Skeleton.Features.Orders;

public class OrderPlace
{
    public class Request : IRequest<Order>
    {
    }

    // #jobperform
    // public class Handler : IRequestHandler<Request, Order>
    // {
    //     private readonly Jobs _jobs;
    //
    //     public Handler(Jobs jobs)
    //     {
    //         _jobs = jobs;
    //     }
    //
    //     public async Task<Order> Handle(Request request, CancellationToken cancellationToken)
    //     {
    //         var order = new Order();
    //         
    //         // place order logic
    //         
    //         _jobs.PerformLater(new OrderPlaced.Request
    //         {
    //             OrderId = order.Id
    //         });
    //         
    //         return order;
    //     }
    // }
    // #jobperform
}