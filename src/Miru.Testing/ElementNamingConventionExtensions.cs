using System;
using System.Linq.Expressions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Reflection;

namespace Miru.Testing
{
    public static class ElementNamingConventionExtensions
    {
        public static string GetName<TModel, TProperty>(
            this IElementNamingConvention elementNaming, 
            Expression<Func<TModel, TProperty>> property)
        {
            return elementNaming.GetName(typeof(TModel), property.ToAccessor());
        }
    }
}