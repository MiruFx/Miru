using System.Collections;
using HtmlTags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-table", Attributes = "for,model")]
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
        if (builder.Source == ModelSource.For)
        {
            htmlTag.Id(ElementNaming.Id(For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType));
        }
        
        if (builder.Source == ModelSource.Model)
        {
            htmlTag.Id(ElementNaming.Id(builder.Model.GetType()));
        }
        
        if (builder.Source == ModelSource.ViewModel)
        {
            htmlTag.Id(ElementNaming.Id(builder.Model.GetType()));
        }
    }

    // TODO: should be:
    //  BeforeHtmlTagGeneration
    //  AfterHtmlTagGeneration
    // public override void Process(TagHelperContext context, TagHelperOutput output)
    // {
    //     var model = GetModel();
    //
    //     if (model is IEnumerable list)
    //     {
    //         if (list.GetEnumerator().MoveNext() == false)
    //         {
    //             output.SuppressOutput();
    //             return;
    //         }
    //     }
    //     
    //     base.Process(context, output);
    // }
    //
    // protected override void BeforeRender(TagHelperOutput output, HtmlTag htmlTag)
    // {
    //     var modelType = GetModelType();
    //     
    //     if (modelType == ModelSource.For)
    //     {
    //         htmlTag.Id(ElementNaming.Id(For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType));
    //     }
    //     
    //     if (modelType == ModelSource.Model)
    //     {
    //         htmlTag.Id(ElementNaming.Id(GetModel().GetType()));
    //     }
    //     
    //     if (modelType == ModelSource.ViewModel)
    //     {
    //         htmlTag.Id(ElementNaming.Id(GetModel().GetType()));
    //     }
    // }
}