using System.Collections;
using HtmlTags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-table", Attributes = ForAttributeName)]
[HtmlTargetElement("miru-table", Attributes = "model")]
[HtmlTargetElement("miru-table")]
public class TableTagHelper : MiruForTagHelper
{
    protected override string Category => nameof(HtmlConfiguration.Tables);

    public override void BeforeHtmlTagGeneration(MiruTagBuilder builder)
    {
        if (builder.Model is IEnumerable list && list.GetEnumerator().MoveNext() == false)
        {
            builder.SuppressOutput = true;
        }
    }

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        // TODO: id can be set in the Convention or in the MiruTagBuilder
        if (builder.Source == ModelSource.For)
        {
            htmlTag.Id($"{ElementNaming.Id(For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType)}-table");
        }
    }
}