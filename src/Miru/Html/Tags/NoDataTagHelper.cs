using System.Collections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("no-data-tag")]
    public class NoDataTagHelper : MiruTagHelperBase
    {
        [HtmlAttributeName("for")]
        public IEnumerable For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (For.GetEnumerator().MoveNext())
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
            }
        } 
    }
}