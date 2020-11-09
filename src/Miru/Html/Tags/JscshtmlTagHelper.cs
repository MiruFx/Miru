using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    public class JscshtmlTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var childHtml = childContent.GetContent();

            if (childHtml.NotEmpty())
            {
                output.TagName = string.Empty;
                
                var htmlInJavascript = HttpUtility.JavaScriptStringEncode(childHtml);

                output.Content.SetHtmlContent(htmlInJavascript);
            }
        } 
    }
}