using System.Threading.Tasks;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-td")]
    public class TdTagHelper : MiruTagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var td = GetHtmlTag(nameof(HtmlConfiguration.Cells));
            
            var childContent = await output.GetChildContentAsync();

            if (childContent.IsEmptyOrWhiteSpace)
            {
                var span = GetHtmlTag(nameof(ElementConstants.Display));
                td.Children.Add(span);
            }
            else
            {
                td.AppendHtml(childContent.GetContent());
                childContent.Clear();
            }
            
            SetOutput(td, output);
        }
    }
}