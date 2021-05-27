using System.Net;
using System.Text;
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
                var formSummaryId = naming.FormSummaryId(validationException.Model);
                
                var html = new StringBuilder();
                
                html.Append(BuildValidationMessageTags(validationException));
                
                html.Append(BuildFormSummaryTag(formSummaryId, validationException));

                return new TurboStreamResult(html.ToString(), HttpStatusCode.UnprocessableEntity);
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
        
        private static string BuildFormSummaryTag(string formSummaryId, MiruValidationException validationException)
        {
            var formSummary = new FormSummaryTag(formSummaryId);

            // TODO: get from htmlconventions
            var turboStreamTag = new TurboStreamTag("replace", formSummaryId)
                .AppendIntoTemplate(formSummary);
            
            validationException.Errors.Each(error => formSummary.Add("div", tag => tag.Text(error.ErrorMessage)));
            
            return turboStreamTag.ToString();
        }
        
        private static StringBuilder BuildValidationMessageTags(MiruValidationException validationException)
        {
            var html = new StringBuilder();

            foreach (var error in validationException.Errors)
            {
                var inputId = ElementNaming.BuildId(error.PropertyName);
                var validationMessageTagId = $"{inputId}-validation";

                var validationMessageTag =
                    new ValidationMessageTag(validationMessageTagId, inputId, error.ErrorMessage);

                var turboStreamTag = new TurboStreamTag("replace", validationMessageTagId)
                    .AppendIntoTemplate(validationMessageTag);

                html.AppendLine(turboStreamTag.ToString());
            }

            return html;
        }
    }
}