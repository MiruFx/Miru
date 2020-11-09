using System.Net;
using HtmlTags;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Mvc
{
    public class HtmlTagResult : ContentResult
    {
        public HtmlTagResult(HtmlTag htmlTag, HttpStatusCode statusCode) : this(htmlTag, (int) statusCode)
        {
        }
        
        public HtmlTagResult(HtmlTag htmlTag, int statusCode = 200)
        {
            ContentType = "text/html";
            Content = htmlTag.ToHtmlString();
            StatusCode = statusCode;
        }
    }
}