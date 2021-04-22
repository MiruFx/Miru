using System;
using HtmlTags.Conventions;
using HtmlTags.Reflection;

namespace Miru.Html
{
    public static class ElementCategoryExpressionExtensions
    {
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
        
        public static ElementActionExpression IfPropertyIs<TType, TOrType>(
            this ElementCategoryExpression expression) =>
                expression.If(req =>
                    req.Accessor.PropertyType == typeof(TType) || req.Accessor.PropertyType == typeof(TOrType));
        
        public static ElementActionExpression IfPropertyTypeAndAttribute<TType, TAttribute>(
            this ElementCategoryExpression expression) where TAttribute : Attribute =>
                expression.If(req => req.Accessor.PropertyType == typeof(TType) && req.Accessor.HasAttribute<TAttribute>());
    }
}