using System;
using System.Linq;
using System.Linq.Expressions;
using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Reflection;
using Microsoft.AspNetCore.Antiforgery;

namespace Miru.Html
{
    public static class HtmlConfigExtensions
    {
        public static ElementActionExpression ForProperty<TModel, TProperty>(
            this ElementCategoryExpression config, 
            Expression<Func<TModel, TProperty>> expression)
        {
            return config.If(m => 
                m.Accessor.OwnerType == typeof(TModel) &&
                m.Accessor.PropertyNames.Contains(expression.ToAccessor().Name));
        }
    }
}