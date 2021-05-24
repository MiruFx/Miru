using System.Net;
using Baseline;
using FluentValidation;
using HtmlTags;
using Miru.Domain;
using Miru.Html;
using Miru.Turbo;
using Miru.Validation;

namespace Miru.Mvc
{
    public class DefaultExceptionResultConfig : ExceptionResultConfiguration
    {
        public DefaultExceptionResultConfig()
        {
            this.MiruTurboFormSummary();
            
            this.MiruDefault();
        }
        
        public void MiruTurboFormSummary()
        {
            When(m => 
                m.Request.IsPost() &&
                m.Request.CanAccept(TurboStreamResult.MimeType) && 
                m.Exception is ValidationException).Respond(m =>
            {
                var validationException = (MiruValidationException) m.Exception;
                var naming = m.GetService<ElementNaming>();
                
                var template = new HtmlTag("template");

                var summaryId = naming.FormSummaryId(validationException.Model).Replace('.', '_');
                
                var summary = new HtmlTag("div")
                    // TODO: parse css selector to HtmlTag (at least basic .class #id attribute
                    .Id(summaryId)
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger")
                    .Attr("data-controller", "form-summary");

                template.Append(summary);
                
                // TODO: get from htmlconventions
                var turboStream = new HtmlTag("turbo-stream")
                    .Attr("action", "replace")
                    .Attr("target", summaryId)
                    .Append(template);
                
                var turboStreamTag = new TurboStreamTag("replace", summaryId)
                    .AppendIntoTemplate(summary);
                
                validationException.Errors.Each(error => summary.Add("div", tag => tag.Text(error.ErrorMessage)));

                return new TurboStreamResult(turboStream, HttpStatusCode.UnprocessableEntity);
            });
            
            When(m => 
                m.Request.IsPost() && 
                m.Request.CanAccept(TurboStreamResult.MimeType)).Respond(m =>
            {
                var template = new HtmlTag("template");

                var summaryId = m.Request.Form["__Summary"];
                
                var summary = new HtmlTag("div")
                    // TODO: parse css selector to HtmlTag (at least basic .class #id attribute
                    // TODO: should not be hardcoded. Should get from HtmlConventions
                    .Id(summaryId)
                    .AddClass("form-summary")
                    .AddClass("alert")
                    .AddClass("alert-danger")
                    .Attr("data-controller", "form-summary");

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