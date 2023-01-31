using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Storages;
using Shouldly;

namespace Miru.Tests;

[ShouldlyMethods]
public static class Extensions
{
    // public static void HtmlShouldBe(this TagHelperOutput tagHelperOutput, string expectedHtml)
    // {
    //     tagHelperOutput.TagName.ShouldBeNull();
    //     tagHelperOutput.PreElement.GetContent().ShouldBe(expectedHtml);
    // }
    
    public static void PostHtmlShouldBe(this TagHelperOutput tagHelperOutput, string expectedHtml)
    {
        tagHelperOutput.PostElement.GetContent().ShouldBe(expectedHtml);
    }
    
    public static void HtmlShouldContain(this TagHelperOutput tagHelperOutput, string expectedHtml)
    {
        tagHelperOutput.PreElement.GetContent().ShouldContain(expectedHtml);
    }
    
    public static IServiceCollection AddMiruCoreTesting(this IServiceCollection services)
    {
        return services
            .AddMiruApp()
            .AddSingleton<TestFixture>()
            .AddSingleton<ITestFixture>(sp => sp.GetRequiredService<TestFixture>())
            .AddMiruSolution(new MiruTestSolution())
            .AddAppTestStorage();
    }
}