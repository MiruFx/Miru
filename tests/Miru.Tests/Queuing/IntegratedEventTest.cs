using MediatR;
using Miru.Domain;
using Miru.Queuing;

namespace Miru.Tests.Queuing;

public class IntegratedEventTest : MiruCoreTest
{
    [Test]
    public void Should_return_notification_name()
    {
        // arrange
        var order = new Order { Id = 2 };
        var orderPlaced = new OrderPlaced(order);
        
        // act
        var notification = orderPlaced.GetEvent();

        // assert
        notification.ToString().ShouldBe("OrderPlaced?OrderId=2");
        (notification as OrderPlaced.Event)!.OrderId.ShouldBe(order.Id);
    }
}

public record OrderPlaced(Order Order) : IDomainEvent, IIntegratedEvent
{
    public record Event(long OrderId) : IntegratedEvent;

    public INotification GetEvent() => new Event(Order.Id);
}

public class Order : Entity
{
}
