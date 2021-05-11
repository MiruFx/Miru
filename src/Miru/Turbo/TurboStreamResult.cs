using System.Net;
using HtmlTags;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Turbo
{
    public class TurboStreamResult : ContentResult
    {
        public const string MimeType = "text/vnd.turbo-stream.html";
        
        public TurboStreamResult(HtmlTag htmlTag, HttpStatusCode statusCode = HttpStatusCode.OK) : 
            this(htmlTag.ToString(), statusCode)
        {
        }
        
        public TurboStreamResult(string html, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            ContentType = MimeType;
            Content = html;
            StatusCode = (int) statusCode;
        }
    }
}