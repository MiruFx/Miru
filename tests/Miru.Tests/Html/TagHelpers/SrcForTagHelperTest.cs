using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class SrcForTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task Should_add_src_attr_for_a_feature_url()
    {
        // arrange
        var tag = CreateTag(new SrcForTagHelper
        {
            For = MakeExpression(new PostShow.Query { Id = 1 })
        });
        
        // act
        var output = await ProcessTagAsync(tag, "img");
            
        // assert
        output.TagName.ShouldBe("img");
        output.Attributes["src"].Value.ShouldBe("/PostShow?Id=1");
    }
        
    public class PostShow
    {
        public class Query
        {
            public long Id { get; set; }
        }    
    }
}