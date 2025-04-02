namespace Miru.Html;

public class PlaceholderAttribute : Attribute
{
    public string Text { get; set; }
    
    public PlaceholderAttribute(string text)
    {
        Text = text;
    }
}