using System;
using HtmlTags.Conventions;

namespace Miru.Html
{
    public static class ElementRequestExtensions
    {
        public static object Get(this ElementRequest elementRequest, Type type)
        {
            var sp = elementRequest.Get<IServiceProvider>();
            var service = sp.GetService(type);

            return service;
        }
    }
}