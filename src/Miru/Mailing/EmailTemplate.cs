namespace Miru.Mailing;

public class EmailTemplate
{
    public string TemplateAt { get; set; }
    public string TemplateName { get; set; }
    public object Model { get; set; }
    public bool IsHtml { get; set; }
}