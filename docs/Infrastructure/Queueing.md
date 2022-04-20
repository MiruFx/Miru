<!--
Introduction
  hangfire
  job, handler, queueing, dashboard
Create
Job
  file
  Request
  Handler
Queueing Jobs
  PerformLater
Processing Jobs
  in the app (startup)
  separated process (miru:queue)
Dashboard
  config
  accessing
  protecting
    (middleware should be after UseSession UseAuthentication)
TODO: Testing
  if is queueing
  the handler
TODO: Examples
TODO: References
-->

[[toc]]

# Queueing

Miru has some Queueing facilities on top of [Hangfire](https://www.hangfire.io/).

## Create a Job

To create a Job, a Maker from MiruCli can be used:

```shell
miru make:job Accounts Account Created
```

![](/Queueing-Make.png)

## Job

A Job is composed by the Request and the Handler. It is a class named after the job being done (e.g. OrderPlaced, UserCreated, and ImageAttached). Request and Handler stay inside as subclasses:

@[code lang=csharp transcludeWith=#job](@/samples/Skeleton/src/Skeleton/Features/Orders/OrderPaid.cs)

The class's file can be placed in the Feature's folder. Examples:

```
/src/App/Features/Orders/OrderPaid.cs
/src/App/Features/Users/UserCreated.cs
```

### Request

The Job's Request holds the input data that will be used by the Handler when the job is processed. It is a class that implements ```IMiruJob```.

@[code lang=csharp transcludeWith=#jobrequest](@/samples/Skeleton/src/Skeleton/Features/Orders/OrderPlaced.cs)

### Handler

A Job's Handler will be called when the queue scheduler execute the job. It is a class that implements ```IRequestHandler```.

@[code lang=csharp transcludeWith=#jobhandler](@/samples/Skeleton/src/Skeleton/Features/Orders/OrderPlaced.cs)

## Queueing Jobs

Queueing jobs are done through ```Jobs``` class:

```csharp
_jobs.PerformLater(job);
```

```csharp
public class Handler : IRequestHandler<Request, Order>
{
    private readonly Jobs _jobs;

    public Handler(Jobs jobs)
    {
        _jobs = jobs;
    }

    public async Task<Order> Handle(Request request, CancellationToken cancellationToken)
    {
        var order = new Order();
        
        // place order logic
        
        _jobs.PerformLater(new OrderPlaced.Request
        {
            OrderId = order.Id
        });
        
        return order;
    }
}
```

## Processing Queued Jobs

The queued jobs are processed by Hangfire's [BackgroundJobServer](https://docs.hangfire.io/en/latest/background-processing/index.html). There are two ways of start the server and keep it running: together with the WebApp or standalone.

### Server with WebApp

In ```src/App/Startup.cs``` ```ConfigureService``` method, add ```AddHangfireServer```:

@[code lang=csharp transcludeWith=#addserver](@/samples/Mong/src/Mong/Startup.cs)

### Server Standalone

Can be started invoking:

```shell
miru queue:run
```

::: warning
Pay attention to not run the ```BackgroundJobServer``` in both ways at the same time.
:::

## Dashboard

Dashboard can be enabled in ```src/App/Startup.cs``` middleware configuration:

@[code lang=csharp transcludeWith=#jobdashboard](@/samples/Mong/src/Mong/Startup.cs)

It will use ```Userfy``` authorization to allow only ```Admin``` access the Dashboard. The default url route is ```/_queue```.

Configurations can be customized using [Hangfire's API](https://docs.hangfire.io/en/latest/configuration/using-dashboard.html).
