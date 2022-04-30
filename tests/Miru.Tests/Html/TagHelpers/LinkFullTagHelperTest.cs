using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class LinkFullTagHelperTest : TagHelperTest
{
    [Test]
    public async Task Should_build_a_full_link_for_a_request()
    {
        // arrange
        var tag = new LinkFullForTagHelper
        {
            FullFor = MakeExpression(new PostShow.Query { Id = 1 }),
            RequestServices = ServiceProvider
        };
        
        // act
        var output = await ProcessTagAsync(tag, "a");
            
        // assert
        output.TagName.ShouldBe("a");
        output.Attributes["href"].Value.ShouldBe("https://mirufx.github.io/PostShow?Id=1");
    }
        
    public class PostShow
    {
        public class Query
        {
            public long Id { get; set; }
        }    
    }
}