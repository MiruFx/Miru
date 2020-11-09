using System.Collections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("*", Attributes = "miru-if-empty")]
    public class IfEmptyTagHelper : TagHelper
    {
        [HtmlAttributeName("miru-if-empty")]
        public IEnumerable Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Model.GetEnumerator().MoveNext())
            {
                output.SuppressOutput();
            }
        }
    }
}