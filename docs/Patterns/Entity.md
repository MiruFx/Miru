# Entity Patterns

## Constructing From Feature Or Entity

Only create a new Entity from a Feature or another Entity.

It helps communicate other developers what's the scope of an Entity creation.

```csharp
public class Order : Entity
{
    // here is clear that the Order is created by New Order feature
    public Order(OrderNew.Command command)
    {
    }

    // here is clear that the Order is created by Buy Again feature
    public Order(OrderBuyAgain.Command command)
    {
    }
}
```

### Testing

