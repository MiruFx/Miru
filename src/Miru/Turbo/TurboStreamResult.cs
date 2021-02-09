using System.Net;
using HtmlTags;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Turbo
{
    public class TurboStreamResult : ContentResult
    {
        public TurboStreamResult(HtmlTag htmlTag, HttpStatusCode statusCode)
            : this(htmlTag, (int) statusCode)
        {
        }

        public TurboStreamResult(HtmlTag htmlTag, int statusCode = 200)
        {
            ContentType = TurboStream.MimeType;
            Content = htmlTag.ToHtmlString();
            StatusCode = statusCode;
        }
    }
}