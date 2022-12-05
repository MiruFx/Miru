using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs.Core;

namespace Miru.Html.HtmlConfigs;

public static class TagConventionsExtensions
{
    public static IConventionAction If<TType>(this ITagConventions convention) => 
        convention.If(req => req.IsAssignable<TType>());
    
    public static IConventionAction If<TType1, TOrType2>(this ITagConventions convention) => 
        convention.If(req => req.Accessor.PropertyType == typeof(TType1)
                            || req.Accessor.PropertyType == typeof(TOrType2));

    public static IConventionAction IfNumber(this ITagConventions convention) =>
        convention.If(req => req.Value is int or long or decimal or float);

    public static IConventionAction IfHas<TAttribute>(this ITagConventions convention) 
        where TAttribute : Attribute =>
            convention.If(req => req.Has<TAttribute>());

    public static IConventionAction IfTag(this ITagConventions convention, Func<TagHelperOutput, bool> condition) =>
            convention.If((tag, _) => condition(tag));
}