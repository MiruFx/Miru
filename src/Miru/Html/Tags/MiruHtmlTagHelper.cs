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
        
        [HtmlAttributeName("set-class")]
        public string SetClass { get; set; }
        
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

            // merge attributes from view tag into html tag
            foreach (var attribute in output.Attributes)
            {
                tag.Attr(attribute.Name, attribute.Value);
            }

            if (SetClass.NotEmpty())
                tag.Class(SetClass);

            BeforeRender(output, tag);
            
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

        protected virtual void BeforeRender(TagHelperOutput output, HtmlTag htmlTag)
        {
        }
        
        protected string GetId()
        {
            var modelType = GetModelType();
            
            if (modelType == ModelType.For)
                return For.Name;
            
            if (modelType == ModelType.Model)
                return ElementNaming.Id(ViewContext.ViewData.Model);
                    
            throw new InvalidOperationException("Modeless view or partial not supported yet");
        }
        
        protected object GetModel()
        {
            var modelType = GetModelType();
            
            if (modelType == ModelType.For)
                return For;
            
            if (modelType == ModelType.Model)
                return ViewContext.ViewData.Model;
                    
            throw new InvalidOperationException("Modeless view or partial not supported yet");
        }
        
        protected HtmlTag GetHtmlTag(string category)
        {
            var modelType = GetModelType();
            
            if (modelType == ModelType.For)
                return HtmlGenerator.TagFor(For, category);
            
            if (modelType == ModelType.Model)
                return HtmlGenerator.TagFor(ViewContext.ViewData.Model, category);
                    
            throw new InvalidOperationException("Modeless view or partial not supported yet");
        }

        protected ModelType GetModelType()
        {
            if (For != null) return ModelType.For;

            if (ViewContext.ViewData.Model != null) return ModelType.Model;

            return ModelType.NoModel;
        }

        protected void SetOutput(HtmlTag htmlTag, TagHelperOutput output)
        {
            htmlTag.MergeAttributes(output.Attributes);
            
            output.TagName = null;
            output.PreElement.SetHtmlContent(htmlTag.ToHtmlString());
            output.Content.Clear();
        }
    }
}