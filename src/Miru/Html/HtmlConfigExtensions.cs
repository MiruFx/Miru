using System;
using System.Linq;
using System.Linq.Expressions;
using HtmlTags.Conventions;
using HtmlTags.Reflection;
using Microsoft.AspNetCore.Http;

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

        public static HtmlConfiguration AddFileEditor(this HtmlConfiguration html)
        {
            html.Editors.IfPropertyIs<IFormFile>().Attr("type", "file");
            return html;
        }
    }
}