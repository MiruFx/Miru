using System;
using System.Collections;
using System.Linq.Expressions;
using Baseline.Reflection;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Miru.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static HtmlTag CheckboxForEnum<T>(
            this IHtmlHelper<T> helper, 
            Expression<Func<T, object>> expression,
            object enumValue) where T : class
        {
            var model = helper.ViewData.Model;
            var tag = HtmlTags.HtmlHelperExtensions.GetGenerator(helper).InputFor(expression);

            var enums2 = (IEnumerable) model.ValueOrDefault(expression);

            foreach (var @enum in enums2)
            {
                if (Convert.ToInt32(@enum) == Convert.ToInt32(enumValue))
                    tag.Attr("checked", "checked");
            }
            
            return tag;
        }
    }
}
