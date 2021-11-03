using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement(Attributes = "miru-if-not")]
    public class IfNotTagHelper : TagHelper
    {
        [HtmlAttributeName("miru-if-not")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Condition)
            {
                output.SuppressOutput();
            }
        }
    }
}