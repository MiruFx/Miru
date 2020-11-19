<!-- 
Page Tests
  what is it? [OK]
  selenium: firefox, chrome [OK]
  run: start server, start selenium [OK]
Configuration
  configure test services [OK]
    browsers [OK]
  configure run
A Page Test
  : pagetest [OK]
  location [OK]
  feature vs page
     e.g. active inactive user
  TODO: what to test: UI, visit links, form, text
TODO: Navigating
TODO: Assert
TODO: Running
TODO: Failed Tests
  screenshot, html, url
-->

[[toc]]

# Page Tests

Page Tests allow you to test the navigation in your application through the browser. Miru uses [Selenium](https://www.selenium.dev/) to interact with the browsers.

At the moment, Miru supports `Chrome` and `Firefox`.

When a Page test is run, Miru starts and stops your application and Selenium automatically.

## Configuration

The Page test configurations are set in `/tests/{AppName}.PageTests/Config/PageTestConfig.cs`.

### Configure Test Services

The services for Page test are set in `PageTestConfig.ConfigureTestServices`

```csharp
public class PageTestsConfig : ITestConfig
{
    public void ConfigureTestServices(IServiceCollection services)
    {
        // import services from {App}.Tests
        services.AddFrom<TestsConfig>();
        
        services.AddPageTesting(options =>
        {
            if (OS.IsWindows)
                options.UseChrome(new ChromeOptions().Incognito());
            else
                options.UseChrome(new ChromeOptions().Incognito().Headless());
        });
    }
}
```

For `Firefox`, use:

```csharp
services.AddPageTesting(options =>
{
    if (OS.IsWindows)
        options.UseFirefox();
    else
        options.UseFirefox(new FirefoxOptions().Headless()); 
});
```

### Configure Run Setup

The method `PageTestConfig.ConfigureRun` configures what is run before and after each test case. `PageTestingDefault` has the default configuration.

```csharp
public void ConfigureRun(TestRunConfig run)
{
    run.PageTestingDefault();
}
```

## A Page Test

### PageTest

Page tests have to inherit `Miru.PageTesting.PageTest`:

```csharp
public class AccountRegisterPageTest : PageTest
{
}
```

### Location

Page tests are located in `/tests/{AppName}.PageTests/Pages/{Feature}/{PageName}PageTest.cs`:

![](/PageTests-Solution.png)

### Page vs Feature Test

While `Feature Tests` pair with a `Feature`, Page Tests pair with pages that users can interact with.

For example, the Page test [UserListPageTest](/samples/Mong/tests/Mong.PageTests/Pages/Admin/) check if is possible to block an user. Although there is a `UserBlockUnblock` feature, the user interact to this feature through `UserList`.

## Example

There are a lot of examples in [Samples](https://github.com/mirufx/miru/tree/master/samples) folder.

This example, test the `TopupNew` page in `Mong` solution:

```csharp
public class TopupNewPageTest : PageTest
{
    private TopupBasic _fix;

    public override async Task GivenAsync()
    {
        _fix = await _.ScenarioAsync<TopupBasic>();
    }

    [Test]
    public void Can_make_new_topup()
    {
        _.LoginAs(_fix.User);
        
        _.Visit<TopupNew.Query>();

        var command = _.Make<TopupNew.Command>();
        
        _.Form<TopupNew.Command>(f =>
        {
            f.Select(m => m.ProviderId, _fix.Provider.Name);
            f.Input(m => m.PhoneNumber, command.PhoneNumber);
            f.Select(m => m.Amount, _fix.Provider.AllAmounts().Second());
            f.Input(m => m.CreditCard, command.CreditCard);
            
            f.Submit();
        });
        
        _.ShouldHaveText("Topup successful");
    }
}
```

