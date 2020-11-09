using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("*", Attributes = "miru-id")]
    public class IdTagHelper : MiruTagHelperBase
    {
        [HtmlAttributeName("miru-id")]
        public object Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Model != null)
            {
                var naming = ViewContext.HttpContext.RequestServices.GetService<ElementNaming>();
                
                output.Attributes.AddOrIgnore("id", naming.Id(Model));
            }
        }
    }
}