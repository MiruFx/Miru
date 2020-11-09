using System;
using System.Linq.Expressions;
using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Reflection;

namespace Miru.Html
{
    public static class ElementGeneratorExtensions
    {
        public static HtmlTag TagFor<TModel>(this IElementGenerator<TModel> generator, TModel model, string category) where TModel : class
        {
            return generator.TagFor(new ElementRequest(new OnlyModelAccessor(model)), category);
        }
        
        public static HtmlTag TagFor<TModel, TProperty>(
            this IElementGenerator<TModel> generator, TModel model, Expression<Func<TModel, TProperty>> func, string category) 
                where TModel : class
        {
            return generator.TagFor(func, category, null, model);
        }
    }
}