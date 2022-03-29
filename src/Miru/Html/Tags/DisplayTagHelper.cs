using System;
using System.Threading.Tasks;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-display", Attributes = ForAttributeName)]
public class DisplayTagHelper : MiruHtmlTagHelper
{
    protected override string Category => ElementConstants.Display;
    
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var spanTag = GetHtmlTag(nameof(ElementConstants.Display));
            
        var childContent = await output.GetChildContentAsync();

        if (childContent.IsEmptyOrWhiteSpace)
        {
            if (For.Model is Enum @enum)
                spanTag.Text(@enum.DisplayName());
            
            SetOutput(spanTag, output);
        }
        else
        {
            spanTag.AppendHtml(childContent.GetContent());
            childContent.Clear();
            SetOutput(spanTag, output);
        }
    }
}