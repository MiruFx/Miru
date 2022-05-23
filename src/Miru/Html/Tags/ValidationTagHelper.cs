using System;
using System.Collections;
using System.Collections.Generic;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-validation", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ValidationTagHelper : MiruForTagHelper
{
    protected override string Category => ElementConstants.ValidationMessage;
}