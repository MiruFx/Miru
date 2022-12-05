using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Tests.Html.HtmlConfigs.Helpers;

[ShouldlyMethods]
public static class Extensions
{
    public static void HtmlShouldBe(this TagHelperOutput tagHelperOutput, string expectedHtml)
    {
        var str = new StringWriter();
        
        tagHelperOutput.WriteTo(str, HtmlEncoder.Default);
        
        var output = str.ToString();
        
        output.ShouldBe(expectedHtml);
    }
    
    public static IServiceCollection AddMiruCoreTesting(this IServiceCollection services)
    {
        return services
            .AddMiruApp()
            .AddSingleton<TestFixture>()
            .AddSingleton<ITestFixture>(sp => sp.GetRequiredService<TestFixture>())
            .AddMiruSolution(new MiruTestSolution());
    }
}