using System.Net;
using System.Text;
using Baseline;
using FluentValidation;
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
                
                html.Append(BuildFormSummaryTag(formSummaryId, validationException, m));

                return new TurboStreamResult(html.ToString(), HttpStatusCode.UnprocessableEntity);
            });
            
            When(m => 
                m.Request.IsPost() && 
                m.Request.CanAccept(TurboStreamResult.MimeType)).Respond(m =>
            {
                var formSummaryId = m.Request.Headers["turbo-form-summary-id"];
                
                var formSummary = new FormSummaryTag(formSummaryId);

                var turboStream = new TurboStreamTag("replace", formSummaryId)
                    .AppendIntoTemplate(formSummary);
                
                App.Log.Error(m.Exception, "An error occurred while processing your request");
                
                var errorMessage = m.Exception is DomainException domainException
                    ? domainException.Message
                    : "An error occurred while processing your request";
                    
                formSummary.Add("div", tag => tag.Text(errorMessage));

                return new TurboStreamResult(turboStream, HttpStatusCode.UnprocessableEntity);
            });
        }

        private static string BuildFormSummaryTag(
            string formSummaryId, 
            MiruValidationException validationException,
            ExceptionResultContext m)
        {
            var formSummary = m.GetService<HtmlGenerator>()
                .FormSummaryFor(validationException.Model)
                .RemoveAttr("hidden");

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