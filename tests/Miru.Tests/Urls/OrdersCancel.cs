using Microsoft.AspNetCore.Mvc;

namespace Miru.Tests.Urls
{
    public class OrdersCancel
    {
        public class Query
        {
            public long Id { get; set; }
        }

        public class Command
        {
            public long OrderId { get; set; }
            public string OrderName { get; set; }
        }

        public class Result
        {
            public long OrderId { get; set; }
            public bool Canceled { get; set; }
        }
            
        [Route("Orders")]
        public class OrdersController : Controller
        {
            [Route("Cancel")] 
            public Command Cancel(Query query) => new Command { OrderId = query.Id, OrderName = $"Order {query.Id}"};

            [Route("Cancel"), HttpPost] 
            public Result Cancel([FromForm] Command command) => new Result { OrderId = command.OrderId, Canceled = true };
        }
    }
}