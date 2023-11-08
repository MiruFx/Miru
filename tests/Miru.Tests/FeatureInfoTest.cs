using MediatR;

namespace Miru.Tests;

public class FeatureInfoTest
{
    [Test]
    public void Should_return_query_string_with_property_and_value_when_long_and_ending_with_id()
    {
        new FeatureInfo(new OrderPurge { OrderId = 123 })
            .GetIdsQueryString()
            .ShouldBe("OrderId=123");
        
        new FeatureInfo(new ShipmentDelivered { ShipmentId = 456, OrderId = 789 })
            .GetIdsQueryString()
            .ShouldBe("ShipmentId=456&OrderId=789");
    }
    
    [Test]
    public void Should_return_key_value_with_property_and_value_when_long_and_ending_with_id()
    {
        new FeatureInfo(new OrderPurge { OrderId = 123 })
            .GetIdsProperties()
            .ShouldBe("OrderId: 123");
        
        new FeatureInfo(new ShipmentDelivered { ShipmentId = 456, OrderId = 789 })
            .GetIdsProperties()
            .ShouldBe("ShipmentId: 456, OrderId: 789");
    }

    [Test]
    public void Should_return_title_with_class_name_and_ids_query_string()
    {
        new FeatureInfo(new OrderPurge { OrderId = 123 })
            .GetTitle()
            .ShouldBe("OrderPurge?OrderId=123");
        
        new FeatureInfo(new ShipmentDelivered { ShipmentId = 456, OrderId = 789 })
            .GetTitle()
            .ShouldBe("ShipmentDelivered?ShipmentId=456&OrderId=789");
        
        new FeatureInfo(new OrderClean())
            .GetTitle()
            .ShouldBe("OrderClean");
    }
    
    [Test]
    public void Should_return_feature_group()
    {
        FeatureInfo
            .GetFeatureGroup(typeof(Playground.Features.Orders.OrderPurge).Namespace)
            .ShouldBe("Orders");
    }
}

public class OrderClean : IBaseRequest
{
}

public class OrderPurge : IBaseRequest
{
    public long OrderId { get; set; }
}

public class ShipmentDelivered : IBaseRequest
{
    public long ShipmentId { get; set; }
    public long OrderId { get; set; }
}