<!--
Getting Started
   serilog
Writing
    app.log: for whole application
    ilogger: to filter specific modules
Configuring
    serilog cfg api
Reading
-->

[[toc]]

# Logging

Miru Logging is based on [Serilog](https://serilog.net/)

## Writing

The most convenient way to write logs is through ```Miru.App.Log```:

```csharp
var topup = new Topup(request, user, provider);
                
App.Log.Debug("Saving db and enqueue job");

await _db.AddSavingAsync(topup);

_jobs.PerformLater(new TopupComplete { TopupId = topup.Id });

App.Log.Information($"Successful created Topup #{topup.Id}");
```

## Configuring

By default, Miru enables only the App and its logs written in ```LogEventLevel.Information``` level.

The configurations can be changed in ```/src/App/Startup.cs```:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMiru<Startup>()
            .AddSerilogConfig(config =>
            {
                // changes your Solution level to Debug
                config.MinimumLevel.Override("SolutionName", LogEventLevel.Debug);
            });
    }
}
```

As ```.AddSerilogConfig(config => ...)``` gives you access to Serilog's configuration API, you can configure your needs checking the [Serilog's Configuration](https://github.com/serilog/serilog/wiki/Configuration-Basics)

## Reading

By default, Miru enables log to be shown in console:

![](/Logging-Console.png)

Other outputs (or sinks as Serilog calls), will depend in what you have configured.