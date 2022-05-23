using System;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using HtmlTags;
using HtmlTags.Conventions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags;

public abstract class MiruForTagHelper : MiruTagHelper
{
    public const string ForAttributeName = "for";
    
    protected abstract string Category { get; }
    
    [HtmlAttributeName("for")]
    public ModelExpression For { get; set; }

    [HtmlAttributeName("model")]
    public object Model { get; set; }
    
    [HtmlAttributeName("add-class")]
    public string AddClass { get; set; }

    [HtmlAttributeName("set-class")]
    public string SetClass { get; set; }
    
    public virtual void BeforeHtmlTagGeneration(MiruTagBuilder builder)
    {
    }

    public virtual void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
    }

    public virtual void AfterSetHtmlContent(MiruTagBuilder builder, HtmlTag htmlTag)
    {
    }
    
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var builder = new MiruTagBuilder(this, context, output);
        
        BeforeHtmlTagGeneration(builder);

        if (builder.SuppressOutput)
        {
            output.SuppressOutput();
            return;
        }
        
        var library = RequestServices.GetRequiredService<HtmlConventionLibrary>();

        var additionalServices = new object[]
        {
            For?.ModelExplorer,
            ViewContext,
            new ElementName(For?.Name)
        };

        // TODO: can be cached? Use HtmlGeneration? If HtmlGenerator does not give same result then
        // move this code to HtmlGenerator
        object ServiceLocator(Type t) => additionalServices.FirstOrDefault(t.IsInstanceOfType) ?? RequestServices.GetService(t);

        var tagGenerator = new TagGenerator(library.TagLibrary, new ActiveProfile(), ServiceLocator);

        var htmlTag = tagGenerator.Build(builder.ElementRequest, Category);
            
        builder.ChildContent = await output.GetChildContentAsync();
            
        if (builder.ChildContent.IsEmptyOrWhiteSpace == false)
        {
            htmlTag.AppendHtml(builder.ChildContent.GetContent());
            htmlTag.Text(string.Empty);
        }

        htmlTag.MergeAttributes(output.Attributes);

        if (AddClass.NotEmpty() && SetClass.IsEmpty())
        {
            htmlTag.AddClasses(AddClass);
        }
        else if (SetClass.IsNotEmpty())
        {
            htmlTag.Class(SetClass);
        }

        var topTag = htmlTag.RenderFromTop();
            
        AfterHtmlTagGeneration(builder, htmlTag);
        
        if (builder.ChildContent.IsEmptyOrWhiteSpace == false)
        {
            builder.ChildContent.Clear();
        }
        
        if (builder.ChildContent.IsModified)
        {
            output.TagName = null;
            output.PreElement.SetHtmlContent(topTag.ToString());
            output.PreContent.Clear();
            output.Content.Clear();
            output.PostElement.Clear();
            output.PostContent.Clear();
        }
        else
        {
            output.TagName = null;
            output.PreElement.SetHtmlContent(topTag.ToString());
        }

        AfterSetHtmlContent(builder, htmlTag);
    }
}