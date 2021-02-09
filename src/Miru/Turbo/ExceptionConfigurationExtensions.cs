using System.Net;
using Baseline;
using FluentValidation;
using HtmlTags;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Miru.Domain;
using Miru.Html;
using Miru.Mvc;
using Miru.Validation;

namespace Miru.Turbo
{
    public static class ExceptionConfigurationExtensions
    {
        public static void MiruTurbo(this ExceptionResultConfiguration _)
        {
            _.When(m => 
                m.Request.IsPost() &&
                m.Request.CanAccept(TurboStream.MimeType) && 
                m.Exception is ValidationException).Respond(m =>
            {
                var validationException = (MiruValidationException) m.Exception;
                var naming = m.GetService<ElementNaming>();
                
                var template = new HtmlTag("template");

                var summaryId = naming.FormSummaryId(validationException.Model);
                
                var summary = new HtmlTag("div")
                    // TODO: parse css selector to HtmlTag (at least basic .class #id attribute
                    .Id(summaryId)
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger");

                template.Append(summary);
                
                // TODO: get from htmlconventions
                var turboStream = new HtmlTag("turbo-stream")
                    .Attr("action", "replace")
                    .Attr("target", summaryId)
                    .Append(template);

                validationException.Errors.Each(error => summary.Add("div", tag => tag.Text(error.ErrorMessage)));

                return new TurboStreamResult(turboStream, HttpStatusCode.UnprocessableEntity);
            });
            
            _.When(m => 
                m.Request.IsPost() && 
                m.Request.CanAccept(TurboStream.MimeType)).Respond(m =>
            {
                var template = new HtmlTag("template");

                var summaryId = m.Request.Form["__Summary"];
                
                var summary = new HtmlTag("div")
                    // TODO: parse css selector to HtmlTag (at least basic .class #id attribute
                    // TODO: should not be hardcoded. Should get from HtmlConventions
                    .Id(summaryId)
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger");

                template.Append(summary);
                
                // TODO: get from htmlconventions
                // TODO: new TurboStreamTag()
                var turboStream = new HtmlTag("turbo-stream")
                    .Attr("action", "replace")
                    .Attr("target", summaryId)
                    .Append(template);

                var errorMessage = m.Exception is DomainException domainException
                    ? domainException.Message
                    : "An error occurred while processing your request";
                    
                summary.Add("div", tag => tag.Text(errorMessage));

                return new TurboStreamResult(turboStream, HttpStatusCode.UnprocessableEntity);
            });
        }
    }
}