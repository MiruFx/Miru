using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using Baseline.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs.Core;
using Miru.Html.Tags;

namespace Miru.Html.HtmlConfigs;

public static class DefaultHtmlConventionsExtensions
{
    public static HtmlConventions AddDefaultHtml(this HtmlConventions html)
    {
        //
        // Inputs
        //
        html.Inputs.Always.Modify((tag, req) =>
        {
            // if tag already has name attribute, we use it to transfer and set the id attribute 
            if (tag.Attributes.TryGetAttribute(HtmlAttr.Name, out var nameAttr))
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.InputIdFromName(nameAttr.Value.ToString()));
            else
                tag.Attributes.SetAttribute(HtmlAttr.Name, req.Naming.InputName(req));
            
            if (tag.Attributes.ContainsName(HtmlAttr.Id) == false)
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.InputId(req));
        });
        
        html.Selects.Always.Modify((tag, req) =>
        {
            // if tag already has name attribute, we use it to transfer and set the id attribute 
            if (tag.Attributes.TryGetAttribute(HtmlAttr.Name, out var nameAttr))
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.InputIdFromName(nameAttr.Value.ToString()));
            else
                tag.Attributes.SetAttribute(HtmlAttr.Name, req.Naming.InputName(req));
            
            if (tag.Attributes.ContainsName(HtmlAttr.Id) == false)
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.InputId(req));
        });

        html.FormSummaries.Always.Modify((tag, req) =>
        {
            if (tag.Attributes.ContainsName(HtmlAttr.Id) == false)
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.FormSummaryId(req.Value));
        });
        
        html.ValidationMessages.Always.Modify((tag, req) =>
        {
            if (tag.Attributes.TryGetAttribute(HtmlAttr.Name, out var nameAttr))
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.InputIdFromName(nameAttr.Value.ToString()) + "-validation");
            
            if (tag.Attributes.ContainsName(HtmlAttr.Id) == false)
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.InputId(req) + "-validation");
            
            tag.Attributes.SetAttribute(HtmlAttr.DataFor, req.Naming.InputId(req));
        });

        //
        // input's type
        //
        html.Inputs.Always.Modify((tag, req) =>
        {
            if (tag.Attributes.ContainsName(HtmlAttr.Type) == false)
            {
                if (req.Name.EndsWith("Id"))
                    tag.Attributes.SetAttribute(HtmlAttr.Type, HtmlAttr.Hidden);
                else if (req.Has<RadioAttribute>())
                    tag.Attributes.SetAttribute(HtmlAttr.Type, HtmlAttr.Radio);
                else if (req.Has<CheckboxAttribute>())
                    tag.Attributes.SetAttribute(HtmlAttr.Type, HtmlAttr.Checkbox);
                else
                    tag.Attributes.SetAttribute(HtmlAttr.Type, HtmlAttr.Text);
            }
        });
            
        // set the input's value
        html.Inputs.Always.Modify((tag, req) =>
        {
            if (tag.Attributes.ContainsName("value") == false && req.Value is { } modelValue)
                tag.Attributes.SetAttribute(HtmlAttr.Value, modelValue);
            
            // radio
            if (tag.Attributes.TryGetAttribute(HtmlAttr.Type, out var attrType) 
                && attrType.ValueIsEqual(HtmlAttr.Radio))
            {
                var attrValue = tag.Attributes.GetValue(HtmlAttr.Value);

                if (req.Value is bool valueBool)
                {
                    if (valueBool.IsEqual(attrValue))
                        tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
                }
                else if (req.Value is IEnumerable list and not string && attrValue is not null)
                {
                    foreach (var item in list)
                        if (attrValue.Equals(item.ToString()))
                            tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
                }
                else if (req.Value is not null && attrValue is not null && attrValue.Equals(req.Value.ToString()))
                    tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
                else if (req.Value is Enum @enum 
                         && int.TryParse(attrValue, out var intValue)
                         && @enum.ToInt().Equals(intValue))
                    tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
            }
            // checkbox
            else if (attrType.ValueIsEqual(HtmlAttr.Checkbox))
            {
                // due to how browser handles checkboxes and submit them
                // we generate two inputs: one checkbox value true and one hidden value false
                // input checkbox
                tag.Attributes.SetAttribute(HtmlAttr.Value, HtmlAttr.True);
                
                if (req.Value is true)
                    tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
                
                // input hidden
                var hidden = new TagBuilder(HtmlAttr.Input) 
                { 
                    TagRenderMode = TagRenderMode.SelfClosing,
                    Attributes = 
                    {
                        // [HtmlAttr.Id] = tag.Attributes.GetValue(HtmlAttr.Id), 
                        [HtmlAttr.Name] = tag.Attributes.GetValue(HtmlAttr.Name),
                        [HtmlAttr.Type] = HtmlAttr.Hidden,
                        [HtmlAttr.Value] = HtmlAttr.False
                    }
                };
                tag.PostElement.AppendHtml(hidden);
                
                // else if (req.Value is IEnumerable list and not string && attrValue is not null)
                // {
                //     foreach (var item in list)
                //         if (attrValue.Equals(item.ToString()))
                //             tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
                // }
                // else if (req.Value is not null && attrValue is not null && attrValue.Equals(req.Value))
                //     tag.Attributes.SetAttribute(HtmlAttr.Checked, HtmlAttr.Checked);
                // else
                //     tag.Attributes.SetAttribute(HtmlAttr.Value, HtmlAttr.False);
            }
            else if (attrType.ValueIsEqual(HtmlAttr.Hidden) && req.Value is ISmartEnum)
            {
                tag.Attributes.SetAttribute(HtmlAttr.Value, req.Value.GetPropertyValue("Value"));
            }
        });
        
        html.Inputs
            .If(x => x.IsAssignable<IFormFile>())
            .Modify(tag => tag.Attributes.SetAttribute(HtmlAttr.Type, "file"));
            
        html.Inputs
            .If(x => x.IsAssignable<List<IFormFile>>())
            .Modify(tag =>
            {
                tag.Attributes.SetAttribute(HtmlAttr.Type, "file");
                tag.Attributes.SetAttribute(HtmlAttr.Multiple, HtmlAttr.Multiple);
            });
        
        html.Inputs
            .If(x => x.PropertyName.EndsWith("Password"))
            .Modify(tag => tag.Attributes.SetAttribute(HtmlAttr.Type, "password"));
        
        //
        // forms
        //
        html.Forms.Always.Modify((tag, req) =>
        {
            if (tag.Attributes.ContainsName(HtmlAttr.Id) == false)
                tag.Attributes.SetAttribute(HtmlAttr.Id, req.Naming.Form(req.Value));
        });
        
        //
        // displays
        //
        html.Displays.Always.Modify((tag, req) =>
        {
            if (req.Value is Enum @enum)
                tag.Content.SetHtmlContent(@enum.DisplayName());
            else if (req.Value is string @string)
                tag.Content.SetHtmlContent(@string);
            else if (req.Value is not null)
                tag.Content.SetHtmlContent(req.Value.ToString());
        });
        
        //
        // labels
        //
        html.Labels.Always.Modify((tag, req) =>
        {
            var childContent = tag.GetChildContentAsync().Result;
            
            tag.Attributes.SetAttribute(HtmlAttr.For, req.Naming.InputId(req));

            if (childContent.IsEmptyOrWhiteSpace && tag.Content.IsEmptyOrWhiteSpace)
                tag.Content.SetContent(req.Name);
        });
        
        //
        // Display labels
        //
        html.DisplayLabels.Always.Modify((tag, req) =>
        {
            var childContent = tag.GetChildContentAsync().Result;
            
            if (childContent.IsEmptyOrWhiteSpace && tag.Content.IsEmptyOrWhiteSpace)
                tag.Content.SetContent(req.Name);
        });
        
        //
        // Table Headers
        // 
        html.TableHeaders.Always.Modify((tag, req) =>
        {
            if (req.Accessor is OnlyModelAccessor)
                return;
            
            // TODO: async api
            var childContent = tag.GetChildContentAsync().Result;

            if (childContent.IsEmptyOrWhiteSpace && tag.Content.IsEmptyOrWhiteSpace)
            {
                tag.Content.SetContent(req.PropertyName);

                if (req.Accessor.HasAttribute<DisplayAttribute>())
                    req.Accessor.ForAttribute<DisplayAttribute>(attr =>
                        tag.Content.SetContent(attr.Name));
            }
            else if (tag.Content.IsEmptyOrWhiteSpace == false)
            {
            }
            else
            {
                tag.Content.SetHtmlContent(childContent.GetContent());
            }
        });
        
        //
        // Table Cells
        // 
        html.TableCells.Always.Modify((tag, req) =>
        {
            var childContent = tag.GetChildContentAsync().Result;

            if (childContent.IsEmptyOrWhiteSpace && req.Value is not IEnumerable or string)
            {
                // TODO: create class to build independent taghelperoutput to be append in others taghelpers
                var tagModifier = req.Get<TagModifier>();
                
                var displayAttributes = new TagHelperAttributeList();

                var displayOutput = new TagHelperOutput(
                    "span", 
                    displayAttributes, 
                    (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
                
                tagModifier.ModifyDisplayFor(req, displayOutput);
                
                var str = new StringWriter();
        
                displayOutput.WriteTo(str, HtmlEncoder.Default);
        
                var output = str.ToString();
                
                tag.Content.SetHtmlContent(output);
            }
            else
            {
                tag.Content.SetHtmlContent(childContent.GetContent());
            }
        });
        
        //
        // validation message
        //
        html.ValidationMessages.Always.Modify((tag, req) =>
        {
            tag.Attributes.SetAttribute(HtmlAttr.Hidden, HtmlAttr.Hidden);
        });

        return html;
    }

    public static HtmlConventions AddLabelDisplayAttribute(this HtmlConventions html)
    {
        html.Labels.Always.Modify((tag, req) =>
        {
            if (tag.IsContentModified)
                req.Accessor.ForAttribute<DisplayAttribute>(attr => tag.Content.SetContent(attr.Name));
        });

        html.DisplayLabels.Always.Modify((tag, req) =>
        {
            if (tag.IsContentModified)
                req.Accessor.ForAttribute<DisplayAttribute>(attr => tag.Content.SetContent(attr.Name));
        });
        
        html.TableHeaders.Always.Modify((tag, req) =>
        {
            if (tag.IsContentModified)
                req.Accessor.ForAttribute<DisplayAttribute>(attr => tag.Content.SetContent(attr.Name));
        });
        
        return html;
    }
}