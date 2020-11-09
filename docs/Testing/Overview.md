<!-- 
Overview
  aaa
  TODO: pyramid
Test Types
  unit, domain
  feature
  page
Test Styles
  many cases per class
  one case per class
Configuring
  services, mocking
  run
    decorations
TODO: Running
  TODO: ide
  TODO: dotnet test
  TODO: miru @test dotnet run
  TODO: miru @pagetest dotnet run
-->

[[toc]]

# Testing Overview

Testing is a very important part of a Miru solution. Miru comes with a lot of facilities to make it easy to write and run.

Miru uses [NUnit](https://nunit.org/), [Shouldly](https://github.com/shouldly/shouldly), and other libraries that will be presented in other pages.

## Base Classes

To get the benefits of Miru Testing, is needed inheriting some base classes:

* `MiruTest` most generic base class
* `DomainTest` for domain unit test
* `FeatureTest` for integrated feature test
* `PageTest` for browser driven system test

## The _ Object.

`MiruTest` contains a protected field named `_` of type `TestFixture`. It's the *Facade* for a lot of test facilities, like making objects, querying database, etc:

```csharp
[Test]
public async Task Can_activate_account()
{
    var user = _.MakeSaving<User>();

    await _.Send(new AccountActivate.Query { Token = user.ConfirmationToken });

    _.Db(db => db.Users.First()).ConfirmationToken.ShouldBeNull();
}
```

### Extensions

You can create your own methods to make the tests easier to write and read:

```csharp
public static class TestFixtureExtensions
{
  public static void LoginAs(this ITestFixture fixture, IUser user)
  {
    MiruTest.Log.Information($"Login as #{user.Id}-{user.Display}");

    fixture.Get<IUserSession>().Login(user);
  }
}
```

```csharp
[Test]
public void User_can_edit_own_password()
{
    // arrange
    var authenticatedUser = _.MakeSavingLogin<User>(
      m => m.HashedPassword = Hash.Create("123456"));

    // ...
}
```

## Arrange, Act & Assert

Miru encourages split a test case in three parts:

* `Arrange`: where all scenarios are prepared
* `Act`: method being tested
* `Assert`: check all expected results

```csharp
[Test]
public async Task Block_user()
{
    // arrange
    var user = _.MakeSaving<User>(m => m.BlockedAt = null);
    
    // act
    await _.Send(new UserBlockUnblock.Command { UserId = user.Id });
    
    // assert
    var saved = _.Db(db => db.Users.First(m => m.Id == user.Id));
    saved.IsBlocked().ShouldBeTrue();
}
```

## Test Types

Miru focus on these types of test: 

* [Domain & Unit Tests](/Testing/DomainUnitTests.md): They are fast tests mocking integrated services
* [Features & Integration Tests](/Testing/FeatureIntegrationTests.md): They are slow tests using real services whenever is possible
* [Page Tests](/Testing/PageTests.md): They are super slow executing through a browser using real services whenever is possible

You can still create your own types.

## Test Styles

Miru has two test styles: `Many Cases Per Class` and `One Case Per Class`.

### Many Cases Per Class

Each method, or "cases", has it own isolated arrange/act/assert:

```csharp
public class AccountRegisterTest : FeatureTest
{
    [Test]
    public async Task Can_register_account()
    {
        // arrange

        // act

        // assert
    }

    [Test]
    public async Task Cant_register_already_registered_account()
    {
        // arrange

        // act

        // assert
    }
}
```

### One Case Per Class

It means that one class will cover one scenario and each method make small asserts. 

It's useful when the scenario is complex and has many side effects.

The Test Class needs to inherit `OneCaseFeatureTest`. 

```csharp
public class TopupNewTest : OneCaseFeatureTest
{
  public override async Task Given()
  {
      // arrange
      
      // act
  }

  [Test]
  public void Credit_card_charged_by_Paypau()
  {
      // assert
  }

  [Test]
  public void New_topup_saved()
  {
      // assert
  }

  [Test]
  public void Topup_job_enqueued()
  {
      // assert
  }
}
```

## Global Configuration

Miru test projects (`Tests` and `PageTests`) come with `TestConfig` class, where is possible to configure Services, and what is run Before and After tests.

TestConfig class needs to inherit `ITestConfig`. By default, it is located at `/tests/{Tests}/Config/TestConfig.cs` or `/tests/{PageTests}/Config/PageTestConfig.cs`:

### Configure Test Services

`TestsConfig.ConfigureTestServices` allows configure services, adding or replacing them:

```csharp
public class TestsConfig : ITestConfig
{
    public virtual void ConfigureTestServices(IServiceCollection services)
    {
        services.AddFeatureTesting()
            .AddSqliteDatabaseCleaner()
            .AddFabrication<MongFabricator>();

        services.Mock<IPayPau>();
        services.Mock<IMobileProvider>();

        services.AddSerilogConfig(cfg =>
        {
            cfg.Testing(LogEventLevel.Debug);
        });
    }
}
```

### Configure Actions Before And After Tests

`TestsConfig` allows configuring what should be run before and after the tests.

Miru already comes with default configurations:

```csharp
public class TestsConfig : ITestConfig
{
  public void ConfigureRun(TestRunConfig run)
  {
      run.TestingDefault();
      run.UserfyRequiresAdmin<User>();
  }
}
```

You can customize them. This is an example:

```csharp
public class TestsConfig : ITestConfig
{
  public void ConfigureRun(TestRunConfig run)
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

      run.BeforeCase<IRequiresAuthenticatedAdmin>(_ =>
      {
          _.MakeSavingLogin<User>(m => m.IsAdmin = true);
      });
  }
}
```

As can be seen in the last `run.BeforeCase<IRequiresAuthenticatedAdmin>`, all tests decorated with `IRequiresAuthenticatedAdmin`, will have a User admin saved before running:

```csharp
public class TopupNewTest : IRequiresAuthenticatedUser
{
}
```