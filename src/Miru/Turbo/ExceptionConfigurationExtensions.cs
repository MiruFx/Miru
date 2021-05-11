using System.Net;
using System.Text;
using Baseline;
using FluentValidation;
using HtmlTags;
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
                m.Request.CanAccept(TurboStreamResult.MimeType) && 
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
        
        public static void MiruTurboForm(this ExceptionResultConfiguration _)
        {
            _.When(m => 
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
            
            _.When(m => 
                m.Request.IsPost() && 
                m.Request.CanAccept(TurboStreamResult.MimeType)).Respond(m =>
            {
                var formSummaryId = m.Request.Headers["turbo-form-summary-id"];
                
                var formSummary = new FormSummaryTag(formSummaryId);

                // TODO: get from htmlconventions
                var turboStream = new TurboStreamTag("replace", formSummaryId)
                    .AppendIntoTemplate(formSummary);

                var errorMessage = m.Exception is DomainException domainException
                    ? domainException.Message
                    : "An error occurred while processing your request";
                    
                formSummary.Add("div", tag => tag.Text(errorMessage));

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