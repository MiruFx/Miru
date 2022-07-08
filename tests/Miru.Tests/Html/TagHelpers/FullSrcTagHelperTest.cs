using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class FullSrcTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task Should_return_full_src_with_url_base()
    {
        // arrange
        var tag = CreateTag(new FullSrcTagHelper
        {
            FullSrc = "/public/logos/miru.png",
        });
        
        // act
        var output = await ProcessTagAsync(tag, "a");
            
        // assert
        output.TagName.ShouldBe("a");
        output.Attributes["src"].Value.ShouldBe("https://mirufx.github.io/public/logos/miru.png");
    }
}