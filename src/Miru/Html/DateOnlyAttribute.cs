using System;

namespace Miru.Html
{
    /// <summary>
    /// Properties with attribute [Humanize] will be shown at the html view with Humanize format
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateOnlyAttribute : Attribute
    {
    }
}