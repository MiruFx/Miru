using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using HtmlTags;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html
{
    public static class HtmlTagExtensions
    {
        public static string Name(this HtmlTag tag)
        {
            return tag.Attr("name");
        }
        
        public static HtmlTag AsCheckBox(this HtmlTag tag)
        {
            return tag.Attr("type", "checkbox");
        }
        
        public static HtmlTag AsTextArea(this HtmlTag tag, int cols, int rows)
        {
            return tag.TagName("textarea")
                .Attr("cols", cols)
                .Attr("rows", rows);
        }
        
        public static HtmlTag Placeholder(this HtmlTag tag, string placeholder)
        {
            return tag.Attr("placeholder", placeholder);
        }

        public static HtmlTag DisableWith(this HtmlTag tag, string text)
        {
            return tag.Data("disable-with", text);
        }

        public static HtmlTag AddPattern(this HtmlTag tag, string pattern)
        {
            return tag.Data("pattern", pattern);
        }

        public static HtmlTag AutoCapitalize(this HtmlTag tag)
        {
            return tag.Data("autocapitalize", "true");
        }

        public static HtmlTag Remote(this HtmlTag tag)
        {
            return tag.Data("remote", "true");
        }
        
        public static HtmlTag RemoteReplace(this HtmlTag tag)
        {
            return tag.Data("remote-replace", "true").Data("type", "html");
        }
        
        public static HtmlTag RemotePost(this HtmlTag tag)
        {
            return tag.Remote().Data("method", "post");
        }
        
        public static HtmlTag RemotePost(this HtmlTag tag, string url)
        {
            return tag.RemotePost().Data("url", url);
        }
        
        public static HtmlTag RemoteGet(this HtmlTag tag, string url)
        {
            return tag.Remote().Data("method", "get").Data("url", url);
        }
        
        public static HtmlTag Cascade(this HtmlTag tag, string inputId)
        {
            return tag
                .Data("type", "html")
                .Data("cascade", inputId);
        }

        public static HtmlTag Post(this HtmlTag tag)
        {
            return tag.Data("method", "post");
        }

        public static HtmlTag Class(this HtmlTag tag, string classes)
        {
            return tag.RemoveAttr("class").Attr("class", classes);
        }

        public static HtmlTag Value(this HtmlTag tag, object value)
        {
            return tag.Value(value.ToString());
        }

        public static HtmlTag Disabled(this HtmlTag tag) => tag.Attr("disabled", "disabled");
        
        public static HtmlString ToJs(this HtmlTag tag)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlDecode(tag.ToString())));
        }
        
        public static string Placeholder(this HtmlTag tag) => tag.Attr("placeholder");
        
        public static HtmlTag AutoFocus(this HtmlTag tag) => tag.Attr("autofocus", "autofocus");
        
        public static string MaxLength(this HtmlTag tag) => tag.Attr("maxlength");
        
        public static HtmlTag MaxLength(this HtmlTag tag, int maxLength) => tag.Attr("maxlength", maxLength.ToString());

        public static HtmlTag MergeAttributes(this HtmlTag tag, TagHelperAttributeList attributes)
        {
            foreach (var attribute in attributes)
            {
                tag.Attr(attribute.Name, attribute.Value);
            }
            
            return tag;
        }
    }
}
