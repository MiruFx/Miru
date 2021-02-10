using System;
using HtmlTags;
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
    }
}