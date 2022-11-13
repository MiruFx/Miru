using Microsoft.AspNetCore.Mvc;

namespace Miru.Tests.Pipelines;

public class OrderPlace
{
    public class Query
    {
        public long OrderId { get; set; }
    }

    public class Command
    {
        public long OrderId { get; set; }
        public string CreditCardNumber { get; set; }
    }

    public class Result
    {
        public bool Success { get; set; }
    }
            
    public class OrdersController : Controller
    {
        [HttpGet("/Orders/{OrderId}/Place")] 
        public Command Place(Query query) => 
            new Command { OrderId = query.OrderId };

        [HttpPost] 
        public Result Place([FromForm] Command command) => 
            new Result { Success = true };
    }
}