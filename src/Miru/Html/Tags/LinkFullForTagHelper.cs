using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Urls;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("a", Attributes = "full-for")]
    public class LinkFullForTagHelper : MiruTagHelper
    {
        [HtmlAttributeName("full-for")]
        public object FullFor { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (FullFor != null)
            {
                var url = RequestServices.GetRequiredService<UrlLookup>();
                
                if (FullFor is ModelExpression modelExpression)
                    output.Attributes.SetAttribute("href", url.FullFor(modelExpression.Model));
                else
                    output.Attributes.SetAttribute("href", url.FullFor(FullFor));
            }
        }
    }
}