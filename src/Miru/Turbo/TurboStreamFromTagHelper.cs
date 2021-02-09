using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Turbo
{
    [HtmlTargetElement("turbo-stream-from")]
    public class TurboStreamFrom : TagHelper
    {
        public string From { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "turbo-cable-stream-source";
            output.Attributes.Add("channel", "Turbo::StreamsChannel");
            output.Attributes.Add("signed-stream-name", From);
        }
    }
}