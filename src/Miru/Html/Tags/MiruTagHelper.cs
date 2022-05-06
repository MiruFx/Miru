using System;
using HtmlTags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

public abstract class MiruTagHelper : MiruForTagHelper
{
    protected string GetId()
    {
        var modelType = GetModelType();
            
        if (modelType == ModelType.For)
            return For.Name;
            
        if (modelType == ModelType.Model)
            return ElementNaming.Id(ViewContext.ViewData.Model);
                    
        throw new InvalidOperationException("Modeless view or partial not supported yet");
    }
        
    protected object GetModel()
    {
        var modelType = GetModelType();
            
        if (modelType == ModelType.For)
            return For;
            
        if (modelType == ModelType.Model)
            return ViewContext.ViewData.Model;
                    
        throw new InvalidOperationException("Modeless view or partial not supported yet");
    }
        
    protected HtmlTag GetHtmlTag(string category)
    {
        var modelType = GetModelType();
            
        if (modelType == ModelType.For)
            return HtmlGenerator.TagFor(For, category);
            
        if (modelType == ModelType.Model)
            return HtmlGenerator.TagFor(ViewContext.ViewData.Model, category);
                    
        throw new InvalidOperationException("Modeless view or partial not supported yet");
    }

    protected ModelType GetModelType()
    {
        if (For != null) return ModelType.For;

        if (ViewContext.ViewData.Model != null) return ModelType.Model;

        return ModelType.NoModel;
    }

    protected void SetOutput(HtmlTag htmlTag, TagHelperOutput output)
    {
        htmlTag.MergeAttributes(output.Attributes);
            
        output.TagName = null;
        output.PreElement.SetHtmlContent(htmlTag.ToHtmlString());
        output.Content.Clear();
    }
}