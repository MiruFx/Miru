using System;
using System.Linq.Expressions;
using Baseline.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Html.Tags;
using OnlyModelAccessor = Miru.Html.Tags.OnlyModelAccessor;

namespace Miru.Html.HtmlConfigs;

public struct ElementRequest
{
    public Accessor Accessor { get; set; }
    public object Model { get; set; }
    public IServiceProvider RequestServices { get; set; }
    
    public string PropertyName => Accessor.FieldName;
    public Type HolderType() => Value == null ? Accessor.DeclaringType : Value?.GetType();
    public object Value => Accessor.GetValue(Model);
    public string Name => Accessor.GetName();

    public ElementNaming Naming => RequestServices.Get<ElementNaming>();
    
    public TService Get<TService>() => RequestServices.Get<TService>();

    public string CreateTag<TModel, TProperty, TMiruTag>(
        TMiruTag tag,
        TModel model,
        Expression<Func<TModel, TProperty>> expression,
        Action<TagHelperOutput> action = null)
        where TMiruTag : MiruTagHelper, new()
    {
        var tagModifier = RequestServices.GetService<TagHelperModifier>();

        tag.Model = model;
        tag.ExFor = ReflectionHelper.GetAccessor(expression);
        
        return tagModifier.Create(tag, model, expression);
    }
    
    public object Get(Type type) => RequestServices.GetService(type);
    
    public static ElementRequest Create(MiruTagHelper tagHelper)
    {
        var model = tagHelper.Model ?? tagHelper.ViewContext?.ViewData.Model;
        
        if (tagHelper.ExFor is not null)
            return new ElementRequest 
            {
                Accessor = tagHelper.ExFor,
                Model = model,
                RequestServices = tagHelper.RequestServices
            };
        
        if (tagHelper.For is not null)
            return new ElementRequest
            {
                Accessor = new ModelMetadataAccessor(tagHelper.For),
                Model = model,
                RequestServices = tagHelper.RequestServices
            };
        
        return new ElementRequest
        {
            Accessor = new OnlyModelAccessor(model),
            Model = model,
            RequestServices = tagHelper.RequestServices
        };
    }

    public bool Has<TAttribute>() where TAttribute : Attribute
    {
        if (Accessor is not null)
            return Accessor.HasAttribute<TAttribute>();
        
        return false;
    }
    
    public TAttribute GetPropertyAttribute<TAttribute>() where TAttribute : Attribute
    {
        if (Accessor is not null)
            return Accessor.GetAttribute<TAttribute>();
        
        return null;
    }
}