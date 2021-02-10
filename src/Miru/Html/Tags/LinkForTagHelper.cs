using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Urls;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("a", Attributes = "for")]
    public class LinkForTagHelper : MiruTagHelper
    {
        [HtmlAttributeName("for")]
        public new object For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (For != null)
            {
                var url = ViewContext.HttpContext.RequestServices.GetService<UrlLookup>();
                
                output.Attributes.SetAttribute("href", url.For(For));
            }
        }
    }
}