<!-- 
Query + Command
  query for command to build form
  command sent by form
Make
  miru make:feature --new

screen
  query
  command
feature
  query 
  command
query
  url
  view
command
  url
  validation
  handler
auth required



-->

# Command

[[toc]]

We are going to review a [Feature](/Features/Overview) of type `Command`, meaning that the feature writes data into the Application.

The feature is [DonationNew](https://github.com/MiruFx/Miru/blob/master/samples/Supportreon/src/Supportreon/Features/Donations/DonationNew.cs) from the sample [Supportreon](https://github.com/MiruFx/Miru/blob/master/samples/Supportreon).

In this feature, **authenticated users** can make a **donation** to a **project**.

## UI

Since a feature is an user action, we can start with the user interface. The `DonationNew` page should look like this:

![](/Example-Command-UI.png)

The page **shows**:

* Project's name

The page has **inputs** for

* Donation's Amount
* User's Credit Card

## Feature Class

In Miru, every feature has a class representing it. It stays in a directory together with other features with the same topic or group.

![](/Example-Command-Feature.png)

```csharp
public class DonationNew
{
}
```

## Query + Command

This feature has this actions

* User goes to the page (Query)
* User fills the inputs and click 'donate' (Command)

A Query is a request to render a page. A Command is a request to write data into the Application.

## Query

We need a class Query to represent the request to render the page:

```csharp
public class DonationNew
{
  public class Query : IRequest<Command>
  {
      public long ProjectId { get; init; }
  }
}
```

A Donation is made for a Project, so `ProjectId` is a input for the Query. `IRequest<Command>` means that the Query will return an instance of `Command`.

### Controller

Controllers in Miru are very simple thin. They do two jobs: configure the route and respond to the request.

```csharp
public class DonationsController : MiruController
{
    [HttpGet("/Projects/{ProjectId:long}/Donations/New")]
    public async Task<Command> New(Query query) => await SendAsync(query);
}
```

The `New` action is `HttpGet`, receives a `Query`, sends to Miru to process, returns a `Command`. Miru will renders a view with the same name as the action. In this case, `New.cshtml`

### Handler

In Miru, Handlers do the actual work of receiving the requests, going to the database, building entities, saving data, talking to other services, and returning a response:

```csharp
public class DonationNew
{
  public class Handler : IRequestHandler<Query, Command>, IRequestHandler<Command, Result>
  {
    // constructor & other methods omitted

    public async Task<Command> Handle(Query request, CancellationToken ct)
    {
        var project = await _db.Projects.ByIdOrFailAsync(request.ProjectId, ct);
        
        return new Command
        {
            ProjectId = request.ProjectId,
            Project = project,
            Amount = project.MinimumDonation
        };
    }
}
```

### View

Now we have the information to render the view, showing the Project's name and inputs for the Command:

```csharp

```

