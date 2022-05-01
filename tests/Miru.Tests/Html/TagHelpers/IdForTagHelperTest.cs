using Miru.Domain;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class IdForTagHelperTest : TagHelperTest
{
    [Test]
    public async Task Should_set_id_attribute_for_entity()
    {
        // arrange
        var entity = new Product { Id = 1234 };
        
        var tag = CreateTag(new IdForTagHelper { IdFor = entity });
        
        // act
        var output = await ProcessTagAsync(tag, "div");
            
        // assert
        output.TagName.ShouldBe("div");
        output.Attributes["id"].Value.ShouldBe("product_1234");
    }

    public class Product : IHasId
    {
        public long Id { get; set; }
    }
}