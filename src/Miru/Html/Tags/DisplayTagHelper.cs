using System;
using System.Threading.Tasks;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-display", Attributes = ForAttributeName)]
public class DisplayTagHelper : MiruForTagHelper
{
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
    }
}