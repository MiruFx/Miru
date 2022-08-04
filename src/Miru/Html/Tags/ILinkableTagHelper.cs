namespace Miru.Html.Tags;

public interface ILinkableTagHelper
{
    object LinkFor { get; set; }
    
    string LinkClass { get; set; }
}