<!-- 
Introduction
TODO: Features
TODO: Routing
  TODO: Feature Routing
    url to feature: scan, urllookup
TODO: ErrorConfig
TODO: ObjectResultConfig
-->

[[toc]]

# Controllers & Routing

Miru Controllers are standard [ASP.NET MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/overview) Controllers. 

In Miru projects though, Controllers are very thin layers responsible to receive an input, call the `Mediator`, and send the output. It also can specify the Route Pattern. 

They are located as subclass inside its `Feature` in `/src/{App}/Features/{Feature}.cs`:

```csharp
public class DonationNew : IMustBeAuthenticated
{
  public class DonationsController : MiruController
  {
    [Route("/Projects/{ProjectId:long}/Donations/New")]
    public async Task<Command> New(Query query) => await Send(query);

    [HttpPost, Route("/Projects/{ProjectId:long}/Donations/New")]
    public async Task<Result> New(Command command) => await Send(command);
  }
}
```

## Url To Features

On the application startup, Miru scans all Controllers making a map of *Controller's Actions Route vs Feature*. With this map, is possible to find the right Url given a Feature type or instance.

For example, in this Feature:

```csharp
public class TopupNew : IMustBeAuthenticated
{
    public class Query : IRequest<Command>
    {
      public string Phone { get; set; }
    }

    public class TopupsController : MiruController
    {
      public async Task<Command> New(Query request) => await Send(request);
    }
}
```

Url can be found doing this:

```csharp
Url.For<Query>() 
// Url: /Topups/New

Url.For(new Query { Phone = "1234-5678" })) 
// Url: /Topups/New?Phone=1234-5678
```