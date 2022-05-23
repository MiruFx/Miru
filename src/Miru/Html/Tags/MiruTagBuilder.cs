using HtmlTags;
using HtmlTags.Conventions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

public class MiruTagBuilder
{
    private static object NoModel = new();

    public TagHelperContext Context { get; }
    public TagHelperOutput Output { get; }
    public object Model { get; set; }
    public ModelExpression ModelExpression { get; set; }
    public ModelSource Source { get; }
    public ElementRequest ElementRequest { get; }
    public bool SuppressOutput { get; set; }
    public TagHelperContent ChildContent { get; set; }

    public object GetModel() => ModelExpression ?? Model;
    public bool HasModel => Model != null;
    
    public MiruTagBuilder(MiruForTagHelper miruTag, TagHelperContext context, TagHelperOutput output)
    {
        Context = context;
        Output = output;
        
        if (miruTag.For != null)
        {
            Source = ModelSource.For;
            Model = miruTag.For.Model;
            ModelExpression = miruTag.For;
            ElementRequest = new ElementRequest(new ModelMetadataAccessor(miruTag.For))
            {
                Model = miruTag.For.Model
            };
        }
        else if (miruTag.Model != null)
        {
            Source = ModelSource.Model;
            Model = miruTag.Model;
            ElementRequest = new ElementRequest(new OnlyModelAccessor(Model))
            {
                Model = Model
            };
        }
        else if (miruTag.ViewContext?.ViewData?.Model != null)
        {
            Source = ModelSource.ViewModel;
            Model = miruTag.ViewContext.ViewData.Model;
            ElementRequest = new ElementRequest(new OnlyModelAccessor(Model))
            {
                Model = Model
            };
        }
        else
        {
            Source = ModelSource.NoModel;
            ElementRequest = new ElementRequest(new OnlyModelAccessor(NoModel))
            {
                Model = NoModel
            };
        }
    }
}