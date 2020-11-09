namespace Miru.Mailing
{
    public class EmailTemplate
    {
        public string File { get; set; }
        public object Model { get; set; }
        public bool IsHtml { get; set; }
    }
}