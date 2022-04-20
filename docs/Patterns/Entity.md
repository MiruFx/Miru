<!--

high level domain helpers
  make other code cleaner
  IsAuthenticated -> the actual entity property
  IsAnonymous -> helper

aggregrate roots
-->

# Entity Patterns

## Constructing From Feature Or Entity

When creating a new Entity, pass through the Entity's constructor the required objects so the Entity take the responsability to correctly set its properties and behaviors:

```csharp
// Order.cs
public class Order : Entity
{
    public Order()
    {
    }
    
    // here is clear that the Order is created by New Order feature
    public Order(OrderNew.Command command)
    {
    }

    // here is clear that the Order is created by Buy Again feature
    public Order(OrderBuyAgain.Command command, Order orderToBuyAgain)
    {
    }
}

// OrderBuyAgain.cs
public class Handler : IRequestHandler<Command, FeatureResult>
{
    private readonly AppDbContext _db;
            
    public Handler(AppDbContext db) => _db = db;
    
    public async Task<Result> Handle(Command request, CancellationToken ct)
    {
        var orderToBuyAgain = await _db.Orders
            .ByIdOrFailAsync(request.OrderToBuyAgainId, ct);
        
        var order = new Order(request, orderToBuyAgain);
        
        await _db.Orders.AddAsync(order, ct);
        
        return new FeatureResult<OrderList>();
    }
}
```

### Testing

