using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Conventions.Elements.Builders;
using HtmlTags.Reflection;
using Miru.Core;
using Miru.Mvc;

namespace Miru.Html
{
    public class HtmlConfiguration : HtmlConventionRegistry
    {
        public static readonly IElementNamingConvention ElementNamingConvention = new DotNotationElementNamingConvention();
        
        public ElementCategoryExpression DisplayLabels =>
            new(Library.TagLibrary.Category(nameof(DisplayLabels)).Profile(TagConstants.Default));

        public ElementCategoryExpression Cells =>
            new(Library.TagLibrary.Category(nameof(Cells)).Profile(TagConstants.Default));
        
        public ElementCategoryExpression TableHeader =>
            new(Library.TagLibrary.Category(nameof(TableHeader)).Profile(TagConstants.Default));
        
        public ElementCategoryExpression Submits =>
            new(Library.TagLibrary.Category(nameof(Submits)).Profile(TagConstants.Default));
        
        public ElementCategoryExpression Forms =>
            new(Library.TagLibrary.Category(nameof(Forms)).Profile(TagConstants.Default));

        public ElementCategoryExpression FormSummaries =>
            new(Library.TagLibrary.Category(nameof(FormSummaries)).Profile(TagConstants.Default));
        
        public ElementCategoryExpression Selects =>
            new(Library.TagLibrary.Category(nameof(Selects)).Profile(TagConstants.Default));

        public HtmlConfiguration()
        {
            // TODO: move default configurations to a extension method to be used in HtmlConfig -> .AddMiruDefault();
            Submits.Always.BuildBy<SubmitBuilder>();

            FormSummaries.Always.BuildBy<FormSummaryBuilder>();
            
            FormSummaries.Always.ModifyWith(m =>
            {
                var naming = m.Get<ElementNaming>();

                m.CurrentTag.Id(naming.FormSummaryId(m.Model));
            });
            
            ValidationMessages.Always.BuildBy<ValidationMessageBuilder>();
            
            Cells.Always.BuildBy<CellBuilder>();
            
            TableHeader.Always.BuildBy<TableHeaderBuilder>();

            Selects.Always.BuildBy<SelectBuilder>();
            Selects.NamingConvention(new DotNotationElementNamingConvention());
            Selects.Always.ModifyWith<AddNameModifier>();
            Selects.Always.ModifyWith<AddIdModifier>();
            
            this.InputHiddenForIds();
            
            // this.InputForBoolean();
            
            this.InputForPassword();
            
            Labels.Modifier<RequiredLabelModifier>();
            
            Labels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));
            
            Labels.IfPropertyNameEnds("Id").ModifyWith(m => m.CurrentTag.Text(m.ElementId.RemoveAtTheEnd(2)));
            
            DisplayLabels.Always.BuildBy<DefaultDisplayLabelBuilder>();
            
            DisplayLabels.ModifyForAttribute<DisplayAttribute>((t, a) => t.Text(a.Name));
            
            Displays
                .IfPropertyNameEnds("Date")
                .ModifyWith(m => m.CurrentTag.Text(m.Value<DateTime>().ToString("dd/MM/yyyy")));
            
            Displays
                .IfPropertyIs<decimal>()
                .ModifyWith(m => m.CurrentTag.Text(m.Value<decimal>().ToString("F2")));
            
            Forms.Always.BuildBy<FormBuilder>();

            Forms
                .If(m => m.Accessor.OwnerType.IsRequestQuery())
                .ModifyWith(m => m.CurrentTag.Attr("method", "get"));
            
            Submits.Always.ModifyTag(tag => tag.DisableWith("Sending..."));
            
            Editors.IfPropertyHasAttribute<RadioAttribute>()
                .ModifyTag(tag => tag.Attr("type", "radio"));
            
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
                .ModifyTag(tag => tag.Attr("type", "checkbox"));
        }
    }
}