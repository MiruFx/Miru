using HtmlTags.Conventions;

namespace Miru.Html;

public static class ElementCategoryExpressionExtensions
{
    public static ElementActionExpression IfPropertyIsNumber(this ElementCategoryExpression expression)
    {
        return expression.If(req => 
            req.Accessor.PropertyType == typeof(int) ||
            req.Accessor.PropertyType == typeof(long) ||
            req.Accessor.PropertyType == typeof(decimal) ||
            req.Accessor.PropertyType == typeof(float));
    }
        
    public static ElementActionExpression IfPropertyNameEnds(this ElementCategoryExpression expression, string text)
    {
        return expression.If(m => m.Accessor.Name.EndsWith(text));
    }

    public static ElementActionExpression IfPropertyNameStarts(this ElementCategoryExpression expression, string text)
    {
        return expression.If(m => m.Accessor.Name.StartsWith(text));
    }
        
    public static ElementActionExpression IfPropertyNameIs(this ElementCategoryExpression expression, string text)
    {
        return expression.If(m => m.Accessor.Name.Equals(text));
    }
        
    // public static ElementActionExpression IfPropertyIs<TType, TOrType>(
    //     this ElementCategoryExpression expression) =>
    //         expression.If(req =>
    //             req.Accessor.PropertyType == typeof(TType) || req.Accessor.PropertyType == typeof(TOrType));
        
    // public static ElementActionExpression IfPropertyTypeAndAttribute<TType, TAttribute>(
    //     this ElementCategoryExpression expression) where TAttribute : Attribute =>
    //         expression.If(req => req.Accessor.PropertyType == typeof(TType) && req.Accessor.HasAttribute<TAttribute>());
        
    // public static ElementActionExpression IfPropertyNameContains<TPropertyType>(
    //     this ElementCategoryExpression expression, 
    //     string text) =>
    //     expression.If(m =>
    //         m.Accessor.Name.Contains(text));
    
    // public static ElementActionExpression PropertyContainNames(
    //     this ElementCategoryExpression expression, 
    //     params string[] names) =>
    //     expression.If(m => names.Any(name => m.Accessor.Name.Contains(name)));

    // public static void TypeAndAttributeModify<TType, TAttribute>(
    //     this ElementCategoryExpression expression,
    //     Action<TType, TAttribute, HtmlTag> func) where TAttribute : Attribute
    // {
    //     expression
    //         .IfPropertyTypeAndAttribute<TType, TAttribute>()
    //         .ModifyWith(x =>
    //         {
    //             var value = x.Value<TType>();
    //             var attribute = x.Accessor.GetAttribute<TAttribute>();
    //
    //             func(value, attribute, x.CurrentTag);
    //         });
    // }
}