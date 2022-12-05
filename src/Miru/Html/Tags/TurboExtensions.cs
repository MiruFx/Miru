using System.Net;
using System.Text;
using Miru.Domain;
using Miru.Html.HtmlConfigs;
using Miru.Mvc;
using Miru.Turbo;
using Miru.Validation;
using ValidationException = FluentValidation.ValidationException;

namespace Miru.Html.Tags;

public static class DefaultExceptionResultConfigExtensions
{
    public static void MiruTurboFormSummary(this ExceptionResultConfiguration config)
    {
        config.When(m => 
            m.Request.IsPost() &&
            m.Request.CanAccept(TurboStreamResult.MimeType) && 
            m.Exception is ValidationException).Respond(m =>
        {
            var validationException = (MiruValidationException) m.Exception;
            var tagModifier = m.GetService<TagHelperModifier>();
                
            var html = new StringBuilder();
                
            html.Append(BuildValidationMessageTags(validationException, tagModifier));
                
            html.Append(BuildFormSummaryTag(validationException.Model, tagModifier, validationException));

            return new TurboStreamResult(html.ToString(), HttpStatusCode.UnprocessableEntity);
        });
            
        config.When(m => 
            m.Request.IsPost() && 
            m.Request.CanAccept(TurboStreamResult.MimeType)).Respond(m =>
        {
            var tagModifier = m.GetService<TagHelperModifier>();
            var formSummaryId = m.Request.Headers["turbo-form-summary-id"];
            
            var html = tagModifier.Modify(
                new FormSummaryTagHelper { Model = m.Request },
                tag =>
                {
                    tag.Attributes.SetAttribute(HtmlAttr.Id, formSummaryId);
                    tag.Attributes.RemoveAll("hidden");

                    App.Log.Error(m.Exception, "An error occurred while processing your request");
                
                    var errorMessage = m.Exception is DomainException domainException
                        ? domainException.Message
                        : "An error occurred while processing your request";
                    
                    tag.Content.AppendHtml($"<div>{errorMessage}</div>");
                
                    // wrap in turbo stream
                    tag.PreElement.SetHtmlContent($"<turbo-stream action=\"replace\" target=\"{tag.Attributes[HtmlAttr.Id].Value}\"><template>");
                    tag.PostElement.SetHtmlContent("</template></turbo-stream>");
                });
            
            return new TurboStreamResult(html, HttpStatusCode.UnprocessableEntity);
        });
    }

    private static string BuildFormSummaryTag(
        object model, 
        TagHelperModifier tagHelperModifier,
        ValidationException validationException)
    {
        return tagHelperModifier.Modify(
            new FormSummaryTagHelper { Model = model },
            tag =>
            {
                tag.Attributes.RemoveAll("hidden");

                foreach (var error in validationException.Errors)
                    tag.Content.AppendHtml($"<div>{error.ErrorMessage}</div>");
                
                // wrap in turbo stream
                tag.PreElement.SetHtmlContent($"<turbo-stream action=\"replace\" target=\"{tag.Attributes[HtmlAttr.Id].Value}\"><template>");
                tag.PostElement.SetHtmlContent("</template></turbo-stream>");
            });
    }

    private static StringBuilder BuildValidationMessageTags(
        MiruValidationException validationException,
        TagHelperModifier tagHelperModifier)
    {
        var html = new StringBuilder();

        foreach (var error in validationException.Errors)
        {
            var inputId = ElementNaming.BuildId(error.PropertyName);
            var validationMessageTagId = $"{inputId}-validation";

            var validationHtml = tagHelperModifier.Modify(
                new ValidationTagHelper(),
                tag =>
                {
                    tag.Attributes.SetAttribute("data-for", inputId);
                    tag.Attributes.SetAttribute("data-controller", "form-input-validation");
                    tag.Attributes.AppendAttr("class", "invalid-feedback");
                    tag.Attributes.SetAttribute(HtmlAttr.Id, validationMessageTagId);
                    tag.Content.SetContent(error.ErrorMessage);
                    
                    // wrap in turbo stream
                    tag.PreElement.SetHtmlContent($"<turbo-stream action=\"replace\" target=\"{validationMessageTagId}\"><template>");
                    tag.PostElement.SetHtmlContent("</template></turbo-stream>");
                });
            
            html.AppendLine(validationHtml);
        }
        
        return html;
    }
}