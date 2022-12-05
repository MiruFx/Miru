// using FluentValidation.Validators;
// using Microsoft.AspNetCore.Razor.TagHelpers;
//
// namespace Miru.Html.HtmlConfigs;
//
// public static class InputHtmlConventionsExtensions
// {
//     public static HtmlConventions AddInputsMaxLength(this HtmlConventions html)
//     {
//         html.Inputs.Always.Modify((tag, req) =>
//         {
//             var validator = req.GetValidator<IMaximumLengthValidator>();
//
//             if (validator is not null) 
//                 tag.Attributes.SetAttribute(HtmlAttr.MaxLength, validator.Max);
//         });
//          
//         return html;
//     }
//     
//     public static HtmlConventions AddInputsCalendar(this HtmlConventions html)
//     {
//         html.Inputs.If(x => x.Has<CalendarAttribute>()).Modify((tag, _) =>
//         {
//             tag.Attributes.SetAttribute("data-controller", "flatpickr");
//             tag.Attributes.SetAttribute("data-flatpickr-date-format", "d.m.Y");
//             tag.Attributes.SetAttribute("data-flatpickr-allow-input", "true");
//         });
//          
//         return html;
//     }
//     
//     public static HtmlConventions AddInputsTextArea(this HtmlConventions html)
//     {
//         html.Inputs.If(x => x.Has<TextAreaAttribute>()).Modify((tag, req) =>
//         {
//             tag.TagName = "textarea";
//             tag.Attributes.RemoveAll("type");
//             tag.Attributes.RemoveAll("value");
//             tag.Content.SetContent(req.Value?.ToString());
//             tag.TagMode = TagMode.StartTagAndEndTag;
//         });
//          
//         return html;
//     }
//     
//     public static HtmlConventions AddInputsPlaceholder(this HtmlConventions html)
//     {
//         html.Inputs.If(x => x.Has<PlaceholderAttribute>()).Modify((tag, req) =>
//         {
//             var attribute = req.GetPropertyAttribute<PlaceholderAttribute>();
//             tag.Attributes.SetAttribute("placeholder", attribute.Text);
//         });
//          
//         return html;
//     }
// }