using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class LinkForExtensionTagHelperTest : MiruTagTesting
{
    // [Test]
    // public async Task Should_add_a_link_into_a_tag_content()
    // {
    //     // arrange
    //     var model = new PostShow.Result { Name = "Getting Started" };
    //     var tag = CreateTagWithFor(
    //         new DisplayTagHelper { LinkFor = new PostEdit.Query { Id = 1 } }, 
    //         model, 
    //         m => m.Name);
    //         
    //     // act
    //     var html = await ProcessTagAsync(tag, "miru-display");
    //         
    //     // assert
    //     html.HtmlShouldBe("<span id=\"Name\"><a href=\"/PostEdit?Id=1\">Getting Started</a></span>");
    // }
        
    public class PostShow
    {
        public class Result
        {
            public string Name { get; set; }
        }
    }
    
    public class PostEdit
    {
        public class Query
        {
            public long Id { get; set; }
        }
    }
}