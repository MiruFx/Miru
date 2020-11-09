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
        public static void AppendAntiForgeryInput(this ElementRequest elementRequest)
        {
            var antiforgeryAccessor = elementRequest.Get<IAntiforgeryAccessor>();

            if (antiforgeryAccessor.HasToken)
            {
                var input = new HtmlTag("input")
                    .Attr("type", "hidden")
                    .Name(antiforgeryAccessor.FormFieldName)
                    .Value(antiforgeryAccessor.RequestToken);
                
                elementRequest.CurrentTag.Append(input);
            }
        }
        
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