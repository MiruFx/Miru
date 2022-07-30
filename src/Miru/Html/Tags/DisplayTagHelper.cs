using System;
using System.Threading.Tasks;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-display", Attributes = ForAttributeName)]
public class DisplayTagHelper : MiruForTagHelper
{
    // [HtmlAttributeName("link-for")]
    // public object LinkFor { get; set; }
    
    protected override string Category => ElementConstants.Display;

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        if (builder.ChildContent.IsEmptyOrWhiteSpace)
        {
            if (For.Model is Enum @enum)
            {
                htmlTag.Text(@enum.DisplayName());
            }
        }

        // if (LinkFor is not null)
        // {
        //     var url = UrlLookup.For(LinkFor);
        //     
        //     var linkTag = new HtmlTag("a").Attr("href", url);
        //
        //     htmlTag.WrapWith(linkTag);
        // }
    }
}