namespace MiruNext.Tests.Framework;

/// <summary>
/// Handling
///     Uploads
/// Request Binding
/// Validation Handling
///     Responding with Turbo view
/// Exception Handling
///     Responding with Turbo view
/// Database Transaction Behavior
///     Rollback when Exception
/// Testing Endpoint
/// </summary>
[TestFixture]
public class EndpointTest
{
    [Test]
    public async Task Should_respond_with_view()
    {
        await using var application = new MainApp();

        var client = application.CreateClient();
       
        var html = await client.GetStringAsync("/Orders");
         
        html.ShouldContain("Orders");
    }
    
    [Test]
    public async Task Should_parse_query_string_into_properties()
    {
        await using var application = new MainApp();

        var client = application.CreateClient();
       
        var output = await client.GetStringAsync("/Orders?Page=99");
        
        output.ShouldContain("Page 99");
    }

    [Test]
    public async Task Should_query_string_with_smart_enum_property()
    {
        await using var application = new MainApp();

        var client = application.CreateClient();
       
        var output = await client.GetStringAsync("/Orders?Status=2");
        
        output.ShouldContain("Status: Paid");
    }
}