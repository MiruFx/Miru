<!--
A Feature
  location [ok]
    feature folders  [ok]
    two levels /Admin/Users and /Users
  structure (class, subclasses)  [ok]
  make:feature
Request & Response
  request: IRequest [ok]
  response [ok]
Mediator
  mediatr [ok]
  send [ok]
  pipeline [ok]
Feature Types [ok]
  query [ok]
  command [ok]
  results
Handlers [ok]
  handler [ok]
  validator
-->
# Overview

[[toc]]

## A Feature

A Feature is an action that a user or client can request to the application (e.g. OrderPlace, UserList, ProductShow, OrderDiscount, etc)

### Naming

A Feature is named: `{FeatureGroup}{Action}`:

| Feature Group | Action        | Feature Name |
| ------------- |-------------  | ----- |
| Orders         | Place        | OrderPlace |
| Orders         | Discount     | OrderDiscount |
| Products       | Show         | ProductShow |

### Location

Miru organizes features using [Feature Folders](https://dev.to/jamesmh/the-life-changing-and-time-saving-magic-of-feature-focused-code-organization-1708):

Features are located in `/src/{AppName}/Features/{FeatureName}.cs`

Instead of group files by artifact type (e.g. Controllers, ViewModels, Views, etc), it is grouped by Feature. Every artifact related to the Feature goes inside the Feature's folder:

![](/Feature-Folders.png)

### Structure

A Feature is a class named after its Feature Group and Action. For example, Place an Order:

```csharp
public class OrderPlace
{
}
```

A Feature can have many components (e.g. Query, Command, Handler, Validator, Controller, Mailable, Job, etc). All these components can be placed inside the class as subclasses:

```csharp
public class OrderPlace
{
    public class Command
    {
    }

    public class Handler
    {
    }

    public class Controller
    {
    }

    public class Mailable
    {
    }

    public class Query
    {
    }
}
```

## Request & Response

Every Feature must have a `Request` class that represents the input data to be handled. Miru uses [MediatR](https://github.com/jbogard/MediatR), which contains an `IRequest` interface to represent a request.

In this example, OrderPlace doesn't return a response:

```csharp
public class OrderPlace
{
    public class Command : IRequest
    {
    }
}
```

When a Feature return information, the `IRequest` needs to be of a `TResponse`:

```csharp
public class OrderShow
{
    public class Query : IRequest<Result>
    {
    }

    public class Result
    {
    }
}
```

## Feature Types

A Feature's Request can be a Query, to ask for information, or a Command, to change some information.

### Query

Query is a Request that ask the application for some data without making changes:

```csharp
public class OrderShow
{
    public class Query : IRequest<Result>
    {
        public long OrderId { get; set; }
    }
}
```

### Command 

Command is a Request that changes data in the application:


```csharp
public class OrderApplyDiscount
{
    public class Command
    {
        public long OrderId { get; set; }
        public decimal Discount { get; set; }
    }
}
```

## Mediator

The `Mediator` from [MediatR](https://github.com/jbogard/MediatR), is the object responsible to receive an `IRequest` and find its `Handler` passing through a pipeline of `Behaviors`.

```csharp
await _mediator.Send(new OrderPlace
{
    Customer = customer,
    Items = items
})
```

If the `IRequest` returns a `Response`:

```csharp
var result = await _mediator.Send(new OrderShow
{
    Id = orderId
})
```

## Handler

A Handler is an object that will process the Request:

```csharp
public class OrderApplyDiscount
{
    public class Command : IRequest
    {
        public long OrderId { get; set; }
        public decimal Discount { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _db;

        public Handler(DbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(Command command, CancellationToken ct)
        {
            var order = await _db.Orders.ByIdAsync(command.OrderId);

            order.ApplyDiscount(command.Discount);

            return Unit.Value;
        }
    }
}

### Other Handlers

Depending of the concern and the library being used, is possible to add other Handlers. An example within Miru is Validation:

@[code lang=csharp transcludeWith=#validator](@/samples/Mong/src/Mong/Features/Accounts/AccountLogin.cs)