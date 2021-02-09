using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Turbo
{
    [HtmlTargetElement("turbo-frame-tag")]
    public class TurboFrameTagHelper : TagHelper
    {
        public string Id { get; set; }
        public string Src { get; set; }
        public string Target { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "turbo-frame";
            
            output.Attributes.Add("id", Id);
            // output.Attributes.Add("src", Src);
            // output.Attributes.Add("target", Target);
        }
    }
}