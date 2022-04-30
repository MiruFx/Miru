using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class FullSrcTagHelperTest : TagHelperTest
{
    [Test]
    public async Task Should_return_full_src_with_url_base()
    {
        // arrange
        var tag = new FullSrcTagHelper
        {
            FullSrc = "/public/logos/miru.png",
            RequestServices = ServiceProvider
        };
        
        // act
        var output = await ProcessTagAsync(tag, "a");
            
        // assert
        output.TagName.ShouldBe("a");
        output.Attributes["src"].Value.ShouldBe("https://mirufx.github.io/public/logos/miru.png");
    }
}