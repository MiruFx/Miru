using System.Net;
using Baseline;
using FluentValidation;
using HtmlTags;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Security;
using Miru.Userfy;
using Vereyon.Web;

namespace Miru.Mvc
{
    public static class ExceptionResultConfigurationExtensions
    {
        public static void MiruDefault(this ExceptionResultConfiguration _)
        {
            _.When(m => m.Exception is NotFoundException && m.Request.IsGet()).Respond(m => 
                new StatusCodeResult((int) HttpStatusCode.NotFound));
            
            // FIXME: maybe should be set in the application
            _.When(m => m.Exception is UnauthorizedException && m.UserSession.IsAnonymous).Respond(m =>
            {
                var userfyOptions = m.GetService<UserfyOptions>();
                
                m.Flash().Warning(userfyOptions.RequiredLoginMessage);
                    
                return new RedirectResult(
                    $"{m.CookieAuthenticationOptions().LoginPath}?ReturnUrl={m.Request.GetEncodedPathAndQuery()}");
            });
            
            _.When(m => m.Exception is UnauthorizedException).Respond(m => 
                new StatusCodeResult((int) HttpStatusCode.Forbidden));

            _.When(m => m.Request.IsAjax() && m.Exception is ValidationException).Respond(m =>
            {
                var validationException = (ValidationException) m.Exception;
                
                var summary = new HtmlTag("div")
                    // TODO: parse css selector to HtmlTag (at least basic .class #id attribute
                    // .Id(ctx.Request.FailTarget().Replace("#", string.Empty))
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger");

                // TODO: get from htmlconventions
                var template = new HtmlTag("template")
                    .Attr("data-page-update", $"update#{m.Request.Headers["X-Miru-Feature"]}-summary")
                    .Append(summary);

                validationException.Errors.Each(error => summary.Add("div", tag => tag.Text(error.ErrorMessage)));

                return new HtmlTagResult(template, HttpStatusCode.BadRequest);
            });

            _.When(m => m.Request.IsAjax() && m.Exception is DomainException).Respond(m =>
            {
                var domainException = (DomainException) m.Exception;
                
                // TODO: get from htmlconventions
                var summary = new HtmlTag("div")
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger")
                    .Text(domainException.Message);

                var template = new HtmlTag("template")
                    .Attr("data-page-update", $"update#{m.Request.Headers["X-Miru-Feature"]}-summary")
                    .Append(summary);
                
                return new HtmlTagResult(template, HttpStatusCode.InternalServerError);
            });
            
            _.When(m => m.Request.IsAjax()).Respond(m =>
            {
                var summary = new HtmlTag("div")
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger")
                    .Text(m.Exception.Message);

                // ctx.Response.FailTarget(".form-summary");

                var template = new HtmlTag("template")
                    .Attr("data-page-update", $"update#{m.Request.Headers["X-Miru-Feature"]}-summary")
                    .Append(summary);
                
                return new HtmlTagResult(template, HttpStatusCode.InternalServerError);
            });
        }
    }
}