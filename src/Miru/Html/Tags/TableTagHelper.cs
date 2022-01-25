using System;
using System.Collections;
using System.Threading.Tasks;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-table", Attributes = "for")]
public class TableTagHelper : MiruHtmlTagHelper
{
    protected override string Category => nameof(HtmlConfiguration.Tables);

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var model = For.Model as IEnumerable;
            
        if (model == null)
            throw new ArgumentException("Attribute 'for' has to be a IEnumerable");

        if (model.GetEnumerator().MoveNext() == false)
        {
            output.SuppressOutput();
            return;
        }
        
        base.Process(context, output);
    }

    protected override void BeforeRender(TagHelperOutput output, HtmlTag htmlTag)
    {
        var modelType = For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType;

        htmlTag.Id(ElementNaming.Id(modelType));
    }
}