using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-antiforgery", Attributes = "name", TagStructure = TagStructure.WithoutEndTag)]
    public class MetaAntiforgeryTagHelper : MiruForTagHelper
    {
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var httpContextAccessor = RequestServices.GetService<IHttpContextAccessor>();
            var antiForgery = RequestServices.GetService<IAntiforgery>();

            if (httpContextAccessor.HttpContext != null)
            {
                output.TagName = "meta";
                output.Attributes.SetAttribute("name", Name);
                output.Attributes.SetAttribute("content",
                    antiForgery.GetAndStoreTokens(httpContextAccessor.HttpContext).RequestToken);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}