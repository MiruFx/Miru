using System.Threading.Tasks;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-td")]
public class TdTagHelper : MiruForTagHelper
{
    protected override string Category => nameof(HtmlConfiguration.Cells);

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        if (builder.ChildContent.IsEmptyOrWhiteSpace)
        {
            var span = HtmlGenerator.TagFor(builder, nameof(ElementConstants.Display));
            htmlTag.Children.Add(span);
        }
    }
}