using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Conventions.Elements.Builders;
using Microsoft.AspNetCore.Http;
using Miru.Core;
using Miru.Mvc;

namespace Miru.Html;

public static class HtmlConfigurationExtensions
{
    public static void AddMiruDefault(this HtmlConfiguration config)
    {
        // Forms
        config.Submits.Always.BuildBy<SubmitBuilder>();

        config.FormSummaries.Always.BuildBy<FormSummaryBuilder>();
            
        config.FormSummaries.Always.ModifyWith(m =>
        {
            var naming = m.Get<ElementNaming>();

            m.CurrentTag.Id(naming.FormSummaryId(m.Model));
        });
            
        config.ValidationMessages.Always.BuildBy<ValidationMessageBuilder>();
            
        config.Forms.Always.BuildBy<FormBuilder>();

        config.Forms
            .If(m => m.Accessor.OwnerType.IsRequestQuery())
            .ModifyWith(m => m.CurrentTag.Attr("method", "get"));

        // Editors
        config.Selects.Always.BuildBy<SelectBuilder>();
        config.Selects.NamingConvention(new DotNotationElementNamingConvention());
        config.Selects.Always.ModifyWith<AddNameModifier>();
        config.Selects.Always.ModifyWith<AddIdModifier>();

        config.InputHiddenForIds();
            
        config.InputForBoolean();
            
        config.InputForPassword();
            
        config.Editors.IfPropertyHasAttribute<RadioAttribute>()
            .ModifyTag(tag => tag.Attr("type", "radio"));
        
        config.Editors.IfPropertyIs<IFormFile>().Attr("type", "file");
            
        config.Editors.IfPropertyIs<List<IFormFile>>().Attr("type", "file");
        config.Editors.IfPropertyIs<List<IFormFile>>().Attr("multiple", "multiple");

        // Labels
        config.Labels.IfPropertyNameEnds("Id").ModifyWith(m => m.CurrentTag.Text(m.ElementId.RemoveAtTheEnd(2)));
        config.Labels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));

        // Displays
        config.Displays
            .IfPropertyNameEnds("Date")
            .ModifyWith(m => m.CurrentTag.Text(m.Value<DateTime>().ToString("dd/MM/yyyy")));
            
        config.Displays
            .IfPropertyIs<decimal>()
            .ModifyWith(m => m.CurrentTag.Text(m.Value<decimal>().ToString("F2")));

        config.Displays.If(x => 
                x.Accessor.PropertyType != typeof(string) && 
                x.Accessor.PropertyType.ImplementsEnumerableOfSomething())
            .ModifyTag(tag => tag.Text(string.Empty));

        // Display Labels
        config.DisplayLabels.Always.BuildBy<DefaultDisplayLabelBuilder>();
            
        config.DisplayLabels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));
            
        // Tables
        config.Tables.Always.BuildBy<TableBuilder>();
        config.Cells.Always.BuildBy<CellBuilder>();
        config.TableHeaders.Always.BuildBy<TableHeaderBuilder>();

        config.Editors.IfPropertyHasAttribute<CheckboxAttribute>()
            .ModifyTag(tag =>
            {
                tag
                    .Attr("type", "checkbox")
                    .Value("true");
            });
        
        // hidden
        config.InputHidden.Always.BuildBy(request =>
        {
            // HtmlTags is not using configured element naming convention for Selects
            // That's way we are doing manually here
            request.ElementId = HtmlConfiguration.ElementNamingConvention
                .GetName(request.Accessor.OwnerType, request.Accessor);
            
            return new HtmlTag("input")
                .Attr("type", "hidden")
                .Attr("value", (request.RawValue ?? string.Empty).ToString());
        });
        config.InputHidden.NamingConvention(new DotNotationElementNamingConvention());
        config.InputHidden.Modifier<AddNameModifier>();
        config.InputHidden.Modifier<AddIdModifier>();
    }
}