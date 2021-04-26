namespace Miru.Html
{
    public static class HtmlConventionExtensions
    {
        public static void InputHiddenForIds(this HtmlConfiguration cfg)
        {
            cfg.Editors
                .IfPropertyNameEnds("Id")
                .ModifyWith(m => m.CurrentTag.Attr("type", "hidden"));
        }
        
        public static void InputForBoolean(this HtmlConfiguration cfg)
        {
            cfg.Editors
                .IfPropertyIs<bool>()
                .ModifyWith(m => m.CurrentTag.Attr("value", true));
        }
        
        public static void InputForPassword(this HtmlConfiguration cfg)
        {
            cfg.Editors
                .If(_ => _.Accessor.Name.Contains("Password"))
                .ModifyWith(m => m.CurrentTag.Attr("type", "password"));
        }
    }
}