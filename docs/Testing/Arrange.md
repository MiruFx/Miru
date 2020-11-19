<!-- 
Arrange
  _
Configuration
  run
  TODO: before/after suite, before/after test, before/after case
  TODO: default configuration
  TODO: custom configuration
Infrastructure
  TODO: clean migration
  TODO: clean queues
  TODO: clean storage
  TODO: clean database
  TODO: save entities  
Fabrication
  make, makemany
  conventions
  TODO: auto convention
TODO: Scenarios

Extending
  extensions methods for TestFixture

-->

[[toc]]

# Arrange

The *Arrange* part of a test case can be quite complex to setup. For example:

* [Run schema changes](/Database/Migrations.md)
* Clean the database
* Clean caches and sessions
* Clean directories
* Create basic seed data (e.g. users)
* Create the test scenario (e.g. entities, mocks returns, and etc)

Miru makes the Arrange part a bit easy.

Most of all features presented in this article can be [configured globally](/Testing/Overview.md#global-configuration)

## Infrastructure

By default, Miru comes configured to arrange infrastructure services, like database, files, etc:

```csharp
public static void TestingDefault(this TestRunConfig run)
{
    run.BeforeSuite(_ =>
    {
        _.MigrateDatabase();
    });

    run.BeforeCase(_ =>
    {
        _.Logout();
        
        _.ClearFabricator();
        _.ClearDatabase();
        _.ClearQueue();
    });
}
```

This configuration is [applied globally](/Testing/Overview.md#global-configuration)

Miru uses [Respawn](https://github.com/jbogard/Respawn) to clean the database.

## Fabrication

Arranging objects like Entities or Requests can be time-consuming and verbose. Miru comes with a module called `Fabrication` which makes it easy to create these objects with some real-world fake data using [Bogus](https://github.com/bchavez/Bogus).

Create these objects can be done through `Make` and `MakeMany`:

```csharp
// arrange
var owner = _.Make<User>();

// Owner:
// Display: Leopoldo30@gmail.com
// Email: Leopoldo30@gmail.com
// Name: Elinor

var project = _.Make<Project>(m => m.User = owner);

// Project:
// Name: Tianna
// Description: Magni nisi animi rem quis quibusdam. Sapiente quod eum. Vitae...
// User:
//   Display: Leopoldo30@gmail.com
//   Email: Leopoldo30@gmail.com
//   Name: Elinor
// Goal: 83
// TotalDonations: 169
// Category:
//   Name: Violet
// IsActive: true
// ...
```

### Configuring

It's possible to configure conventions for how *Fabricator* make objects. The configurations are set at `/tests/{AppName.Tests}/{AppName}Fabricator.cs`:

```csharp
public class SupportreonFabricator : Fabricator
{
    public SupportreonFabricator(FabSupport context) : base(context)
    {
        Fixture.AddConvention(conv =>
        {
            conv
              .IfPropertyIs<Project>(p => p.MinimumDonation)
              .Use(f => f.Finance.Amount(max: 10));
        }
    }
}           
```

In the code above, `f` inside method `Use` is a `Faker` object from [Bogus](https://github.com/bchavez/Bogus). It allows generating random quite realistic data.

```csharp
var project = _.Make<Project>();

// Projects:
// - Name: Earlene
//   MinimumDonation: 9.83
// - Name: Rigoberto
//   MinimumDonation: 8.82
// - Name: Uriah
//   MinimumDonation: 8.76
```

### Making

To make objects `Make` and `MakeMany` methods can be used:

```csharp
var project = _.Make<Project>();

// Can set properties:
var admin = _.Make<User>(m => m.IsAdmin = true);

// Create 2 users:
var users = _.MakeMany<User>(2, m => m.IsAdmin = false);
```

## Your Extensions

Arranging the same thing a lot of times can get repetitive and verbose. When this happens, you can create Extensions Methods for `TestFixture`.

For example, some tests need to fabricate an `User`, save, and login:

```csharp
// arrange
var user = _.Make<User>();
await _.SaveAsync(user);
_.LoginAs(user);
```

Can be replaced with:

```csharp
// arrange
var user = _.MakeSavingLogin<User>();
```

The Extension Method looks like this:

```csharp
public static TUser MakeSavingLogin<TUser>(this ITestFixture fixture, TUser user) 
  where TUser : class, IUser
{
    MiruTest.Log.Debug(() => $"Saving: {user}");
    
    fixture.Save(user);
    
    fixture.LoginAs(user);

    return user;
}
```

