<!-- 
Intro
Pipeline
  intro
  send, publish
  TODO picture
Behaviors
  what is a behavior
-->
# Pipeline & Behaviors

[[toc]]

Miru uses [MediatR](https://github.com/jbogard/MediatR) for the Pipeline and Behaviors.

Consider this code when a Feature is sent to the Mediator:

```csharp
public class AccountLogin 
{
    public class Command : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }  
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken ct)
        {
            return new Result();
        }
    }

    public class AccountsController : MiruController
    {
        [HttpPost]
        public async Task<Result> Login(Command command) => await SendAsync(command);
    }
}
```

When `await SendAsync(command)` is called, the object `AccountLogin.Command` will pass through a [chain of responsability](https://refactoring.guru/design-patterns/chain-of-responsibility). It means that `AccountLogin.Command` will pass through a chain of `Behaviors`, for example Logging, Database Transaction, Authorization, Validation, etc.

## Pipeline

The Pipeline is the chain of `Behaviors`. It is configured in `/src/{AppName}/Startup.cs`:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()
            // pipeline
            .AddDefaultPipeline<Startup>()
    }
}
```

The `.AddDefaultPipeline()` adds these behaviors:

```csharp
services.AddPipeline<TAssemblyOfType>(_ =>
{
    _.UseBehavior(typeof(LogBehavior<,>));
    _.UseBehavior(typeof(DumpRequestBehavior<,>));
    _.UseBehavior(typeof(SetUserBehavior<,>));
    _.UseBehavior(typeof(TransactionBehavior<,>));
    _.UseBehavior(typeof(AuthorizationBehavior<,>));
    _.UseBehavior(typeof(ValidationBehavior<,>));
});
```

`LogBehavior` will be called first, then `DumpRequestBehavior` until call all of them ending in `ValidationBehavior`. After that, the Feature Handler is called.

It's possible to customize the Pipeline by replacing `AddDefaultPipeline` with your own chain of behaviors: 

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()
            // pipeline
            .AddPipeline<Startup>(_ =>
            {
                _.UseBehavior(typeof(YourBehavior<,>));
                _.UseBehavior(typeof(LogBehavior<,>));
                _.UseBehavior(typeof(YourOtherBehavior<,>));
                _.UseBehavior(typeof(TransactionBehavior<,>));
                _.UseBehavior(typeof(AuthorizationBehavior<,>));
                _.UseBehavior(typeof(ValidationBehavior<,>));
            });
    }
}
```

## Behaviors

Behavior is a class that can perform some task before and after the next behavior is called.

This is the structure of a Behavior:

```csharp
public class ExampleBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        // call the next behavior in the pipeline
        var response = await next();

        // return response to the behavior that called this ExampleBehavior
        return response;
    }
}
```

For example, this is the `DumpRequestBehavior` from Miru:

```csharp
public class DumpRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        request.LogIt();
        var response = await next();
        return response;
    }
}
```