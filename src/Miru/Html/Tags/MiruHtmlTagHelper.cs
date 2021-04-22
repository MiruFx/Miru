using System;
using System.Linq;
using HtmlTags;
using HtmlTags.Conventions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    public abstract class MiruHtmlTagHelper : HtmlTagTagHelper
    {
        private IServiceProvider _requestServices;

        [HtmlAttributeNotBound]
        public HtmlGenerator HtmlGenerator => RequestServices.GetService<HtmlGenerator>();
        
        [HtmlAttributeNotBound]
        public ElementNaming ElementNaming => RequestServices.GetService<ElementNaming>();
        
        [HtmlAttributeNotBound]
        public IServiceProvider RequestServices
        {
            get => _requestServices ?? ViewContext.HttpContext.RequestServices;
            set => _requestServices = value;
        }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (For == null)
                throw new InvalidOperationException(
                    "Missing or invalid 'for' attribute value. Specify a valid model expression for the 'for' attribute value.");

            var request = new ElementRequest(new ModelMetadataAccessor(For))
            {
                Model = For.Model
            };

            var library = RequestServices.GetService<HtmlConventionLibrary>();

            var additionalServices = new object[]
            {
                For.ModelExplorer,
                ViewContext,
                new ElementName(For.Name)
            };

            object ServiceLocator(Type t) => additionalServices.FirstOrDefault(t.IsInstanceOfType) ?? RequestServices.GetService(t);

            var tagGenerator = new TagGenerator(library.TagLibrary, new ActiveProfile(), ServiceLocator);

            var tag = tagGenerator.Build(request, Category);
            
            var childContent = output.GetChildContentAsync().GetAwaiter().GetResult();
            
            if (childContent.IsEmptyOrWhiteSpace == false)
            {
                tag.AppendHtml(childContent.GetContent());
                tag.Text(string.Empty);
                childContent.Clear();
            }

            foreach (var attribute in output.Attributes)
            {
                tag.Attr(attribute.Name, attribute.Value);
            }

            if (childContent.IsModified)
            {
                output.TagName = null;
                output.PreElement.AppendHtml(tag);
                output.PreContent.Clear();
                output.Content.Clear();
                output.PostElement.Clear();
                output.PostContent.Clear();
            }
            else
            {
                output.TagName = null;
                output.PreElement.AppendHtml(tag);
            }
        }
    }
}