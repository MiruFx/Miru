using Microsoft.AspNetCore.Http;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputFileTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_file_input_for_form_file_property()
    {
        // arrange
        var model = new Command();
        var tag = TagWithFor(new InputTagHelper(), model, m => m.File);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""File"" id=""File"" type=""file"" />");
    }
   
    public class Command
    {
        public long Id { get; set; }
        public IFormFile File { get; set; }
    }
}