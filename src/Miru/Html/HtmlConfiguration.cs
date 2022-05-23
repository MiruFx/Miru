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

public class HtmlConfiguration : HtmlConventionRegistry
{
    public static readonly IElementNamingConvention ElementNamingConvention = new DotNotationElementNamingConvention();
        
    public ElementCategoryExpression DisplayLabels =>
        new(Library.TagLibrary.Category(nameof(DisplayLabels)).Profile(TagConstants.Default));

    public ElementCategoryExpression Cells =>
        new(Library.TagLibrary.Category(nameof(Cells)).Profile(TagConstants.Default));
        
    public ElementCategoryExpression TableHeaders =>
        new(Library.TagLibrary.Category(nameof(TableHeaders)).Profile(TagConstants.Default));
        
    public ElementCategoryExpression Tables =>
        new(Library.TagLibrary.Category(nameof(Tables)).Profile(TagConstants.Default));

    public ElementCategoryExpression Submits =>
        new(Library.TagLibrary.Category(nameof(Submits)).Profile(TagConstants.Default));
        
    public ElementCategoryExpression Forms =>
        new(Library.TagLibrary.Category(nameof(Forms)).Profile(TagConstants.Default));

    public ElementCategoryExpression FormSummaries =>
        new(Library.TagLibrary.Category(nameof(FormSummaries)).Profile(TagConstants.Default));
        
    public ElementCategoryExpression Selects =>
        new(Library.TagLibrary.Category(nameof(Selects)).Profile(TagConstants.Default));
    
    private ElementCategoryExpression InputHidden =>
        new(Library.TagLibrary.Category(nameof(InputHidden)).Profile(TagConstants.Default));

    public HtmlConfiguration()
    {
        // TODO: move default configurations to a extension method to be used in HtmlConfig -> .AddMiruDefault();
            
        // Forms
        Submits.Always.BuildBy<SubmitBuilder>();

        FormSummaries.Always.BuildBy<FormSummaryBuilder>();
            
        FormSummaries.Always.ModifyWith(m =>
        {
            var naming = m.Get<ElementNaming>();

            m.CurrentTag.Id(naming.FormSummaryId(m.Model));
        });
            
        ValidationMessages.Always.BuildBy<ValidationMessageBuilder>();
            
        Forms.Always.BuildBy<FormBuilder>();

        Forms
            .If(m => m.Accessor.OwnerType.IsRequestQuery())
            .ModifyWith(m => m.CurrentTag.Attr("method", "get"));

        // Editors
        Selects.Always.BuildBy<SelectBuilder>();
        Selects.NamingConvention(new DotNotationElementNamingConvention());
        Selects.Always.ModifyWith<AddNameModifier>();
        Selects.Always.ModifyWith<AddIdModifier>();

        this.InputHiddenForIds();
            
        this.InputForBoolean();
            
        this.InputForPassword();
            
        Editors.IfPropertyHasAttribute<RadioAttribute>()
            .ModifyTag(tag => tag.Attr("type", "radio"));
        
        Editors.IfPropertyIs<IFormFile>().Attr("type", "file");
            
        Editors.IfPropertyIs<List<IFormFile>>().Attr("type", "file");
        Editors.IfPropertyIs<List<IFormFile>>().Attr("multiple", "multiple");

        // Labels
        Labels.IfPropertyNameEnds("Id").ModifyWith(m => m.CurrentTag.Text(m.ElementId.RemoveAtTheEnd(2)));
        Labels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));

        // Displays
        Displays
            .IfPropertyNameEnds("Date")
            .ModifyWith(m => m.CurrentTag.Text(m.Value<DateTime>().ToString("dd/MM/yyyy")));
            
        Displays
            .IfPropertyIs<decimal>()
            .ModifyWith(m => m.CurrentTag.Text(m.Value<decimal>().ToString("F2")));

        Displays.If(x => 
                x.Accessor.PropertyType != typeof(string) && 
                x.Accessor.PropertyType.ImplementsEnumerableOfSomething())
            .ModifyTag(tag => tag.Text(string.Empty));

        // Display Labels
        DisplayLabels.Always.BuildBy<DefaultDisplayLabelBuilder>();
            
        DisplayLabels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));
            
        // Tables
        Tables.Always.BuildBy<TableBuilder>();
        Cells.Always.BuildBy<CellBuilder>();
        TableHeaders.Always.BuildBy<TableHeaderBuilder>();

        // Editors.If(req => req.Accessor.HasAttribute<RadioAttribute>() && req.Accessor.PropertyType == typeof(bool))
        //     .BuildBy(m =>
        //     {
        //         var tag = new HtmlTag("input");
        //         var value = m.Value<bool>();
        //         
        //         tag.Attr("type", "radio");
        //         tag.Attr("value", m.RawValue != null ? value : "false");
        //         
        //         if (tag.Attr("value").ToBool() == value)
        //             tag.Attr("checked", "checked");
        //         else
        //             m.CurrentTag.RemoveAttr("checked");
        //
        //         return m.CurrentTag;
        //     });

        Editors.IfPropertyHasAttribute<CheckboxAttribute>()
            .ModifyTag(tag =>
            {
                tag
                    .Attr("type", "checkbox")
                    .Value("true");
            });
        
        // hidden
        InputHidden.Always.BuildBy(request =>
        {
            // HtmlTags is not using configured element naming convention for Selects
            // That's way we are doing manually here
            request.ElementId = ElementNamingConvention
                .GetName(request.Accessor.OwnerType, request.Accessor);
            
            return new HtmlTag("input")
                .Attr("type", "hidden")
                .Attr("value", (request.RawValue ?? string.Empty).ToString());
        });
        InputHidden.NamingConvention(new DotNotationElementNamingConvention());
        InputHidden.Modifier<AddNameModifier>();
        InputHidden.Modifier<AddIdModifier>();
    }
}