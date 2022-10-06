using MediatR;
using Miru.Domain;
using Miru.Queuing;

namespace Miru.Tests.Queuing;

public class MiruNotificationTest : MiruCoreTesting
{
    [Test]
    public void Should_return_notification_name()
    {
        // arrange
        var order = new Order { Id = 2 };
        var orderPlaced = new OrderPlaced { Order = order };
        
        // act
        var notification = orderPlaced.GetNotification();

        // assert
        notification.ToString().ShouldBe("OrderPlaced?OrderId=2");
    }
}

public class OrderPlaced : IDomainEvent, IEnqueuedEvent
{
    public Order Order { get; init; }

    public INotification GetNotification() => 
        new Event { OrderId = Order.Id };
        
    public class Event : MiruNotification
    {
        public long OrderId { get; init; }
    }
}

public class Order : Entity
{
}
