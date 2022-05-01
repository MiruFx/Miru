using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "id-for")]
public class IdForTagHelper : MiruHtmlTagHelper
{
    [HtmlAttributeName("id-for")]
    public object IdFor { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var id = ElementNaming.Id(IdFor);
        
        output.Attributes.SetAttribute("id", id);
    }

    protected override string Category { get; }
}