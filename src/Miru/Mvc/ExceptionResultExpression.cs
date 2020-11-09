using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Miru.Mvc
{
    public class ExceptionResultExpression
    {
        internal Func<ExceptionResultContext, IActionResult> RespondWith { get; set; }

        public Func<ExceptionResultContext, bool> When { get; set; }
        
        // TODO: Extension method
        public void Respond(Func<ExceptionResultContext, HtmlTagResult> respondWith)
        {
            Respond<ContentResult>(respondWith);
        }
        
        public void Respond<TActionResult>(Func<ExceptionResultContext, TActionResult> respondWith) where TActionResult : IActionResult
        {
            RespondWith = ctx => respondWith(ctx);
        }
    }
}