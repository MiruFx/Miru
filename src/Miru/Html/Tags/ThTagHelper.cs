using System.Threading.Tasks;
using HtmlTags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-th")]
public class ThTagHelper : MiruForTagHelper
{
    protected override string Category => nameof(HtmlConfiguration.TableHeaders);

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        if (builder.ChildContent.IsEmptyOrWhiteSpace && builder.HasModel)
        {
            var span = HtmlGenerator.TagFor(For, nameof(HtmlConfiguration.DisplayLabels));
            htmlTag.Children.Add(span);
        }
    }
}