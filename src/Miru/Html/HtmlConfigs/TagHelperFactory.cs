using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Baseline.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.Tags;

namespace Miru.Html.HtmlConfigs;

public class TagHelperModifier
{
    private static readonly DefaultTagHelperContent TagHelperContent = new();
    
    private readonly IServiceProvider _sp;

    public TagHelperModifier(IServiceProvider sp)
    {
        _sp = sp;
    }

    public string Create<TModel, TProperty, TMiruTag>(
        TMiruTag tag,
        TModel model,
        Expression<Func<TModel, TProperty>> expression,
        Action<TagHelperOutput> action = null)
        where TMiruTag : MiruForTagHelper, new()
    {
        tag.Model = model;
        tag.ExFor = ReflectionHelper.GetAccessor(expression);

        return Modify(tag, action);
    }

    public string Modify<TMiruTag>(
        TMiruTag tag,
        Action<TagHelperOutput> action = null) 
            where TMiruTag : MiruForTagHelper, new()
    {
        tag.RequestServices = _sp;
        
        var tagAttributes = new TagHelperAttributeList();
        
        var output = new TagHelperOutput(
            HtmlAttr.Span,
            tagAttributes,
            (_, _) =>
            {
                TagHelperContent.SetHtmlContent(string.Empty);
                return Task.FromResult<TagHelperContent>(TagHelperContent);
            });

        var context = new TagHelperContext(
            tagAttributes,
            new Dictionary<object, object>(),
            output.GetHashCode().ToString());

        tag.Process(context, output);
        
        action?.Invoke(output);
        
        var str = new StringWriter();
        
        output.WriteTo(str, HtmlEncoder.Default);
        
        return str.ToString();
    }
}