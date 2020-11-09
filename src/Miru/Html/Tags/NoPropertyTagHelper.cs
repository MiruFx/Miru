using System;
using HtmlTags.Conventions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    public abstract class NoPropertyTagHelper : MiruTagHelperBase
    {
        protected abstract string Category { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var request = new ElementRequest(new OnlyModelAccessor(ViewContext.ViewData.Model))
            {
                Model = ViewContext.ViewData.Model
            };

            var library = ViewContext.HttpContext.RequestServices.GetService<HtmlConventionLibrary>();

            object ServiceLocator(Type t) => ViewContext.HttpContext.RequestServices.GetService(t);

            var tagGenerator = new TagGenerator(library.TagLibrary, new ActiveProfile(), ServiceLocator);

            var tag = tagGenerator.Build(request, Category);

            foreach (var attribute in output.Attributes)
            {
                tag.Attr(attribute.Name, attribute.Value);
            }

            output.TagName = null;
            output.PreElement.AppendHtml(tag);
        }
    }
}