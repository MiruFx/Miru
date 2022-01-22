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

    protected override void BeforeRender(TagHelperOutput output, HtmlTag htmlTag)
    {
        var model = For.Model as IEnumerable;
            
        if (model == null)
            throw new ArgumentException("Attribute 'for' has to be a IEnumerable");

        if (model.GetEnumerator().MoveNext() == false)
        {
            output.SuppressOutput();
            return;
        }
        
        var modelType = For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType;

        htmlTag.Id(ElementNaming.Id(modelType));
        htmlTag.MergeAttributes(output.Attributes);
        
        var childContent = output.GetChildContentAsync().GetAwaiter().GetResult();

        htmlTag.AppendHtml(childContent.GetContent());
    }

    // public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    // {
    //     var model = For.Model as IEnumerable;
    //         
    //     if (model == null)
    //         throw new ArgumentException("Attribute 'for' has to be a IEnumerable");
    //
    //     if (model.GetEnumerator().MoveNext() == false)
    //     {
    //         output.SuppressOutput();
    //     }
    //     
    //     var modelType = For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType;
    //     var table = GetHtmlTag(nameof(HtmlConfiguration.Tables));
    //
    //     table.Id(ElementNaming.Id(modelType));
    //     table.MergeAttributes(output.Attributes);
    //     
    //     var childContent = await output.GetChildContentAsync();
    //
    //     table.AppendHtml(childContent.GetContent());
    //     
    //     SetOutput(table, output);
    // }
}