using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("script", Attributes = "mix-src", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MixScriptTagHelper : MiruForTagHelper
    {
        [HtmlAttributeName("mix-src")]
        public string Src { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var assetsMap = RequestServices.GetService<AssetsMap>();
            output.Attributes.SetAttribute("src", assetsMap.For(Src));
        }
    }
    
    [HtmlTargetElement("link", Attributes = "mix-href", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MixLinkTagHelper : MiruForTagHelper
    {
        [HtmlAttributeName("mix-href")]
        public string Href { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var assetsMap = RequestServices.GetService<AssetsMap>();
            output.Attributes.SetAttribute("href", assetsMap.For(Href));
        }
    }
}