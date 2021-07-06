using System.Threading.Tasks;
using Miru.Html.Tags;
using NUnit.Framework;

namespace Miru.Tests.Html.TagHelpers
{
    public class ThTagHelperTest : TagHelperTest
    {
        [Test]
        public async Task Can_create_empty_header()
        {
            // arrange
            var model = new Result();
            var tag = CreateTag(new ThTagHelper { Model = model });

            // act
            var html = await ProcessTagAsync(tag, "miru-th");
            
            // assert
            html.HtmlShouldBe("<th></th>");
        }

        public class Result
        {
            public string Name { get; set; }
        }
    }
}