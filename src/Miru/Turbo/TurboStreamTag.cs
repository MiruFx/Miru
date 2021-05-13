using HtmlTags;

namespace Miru.Turbo
{
    public class TurboStreamTag : HtmlTag
    {
        private readonly HtmlTag _templateTag;

        public TurboStreamTag(string action, string targetId) : base("turbo-stream")
        {
            Attr("action", action);
            Attr("target", targetId);
            
            _templateTag = new HtmlTag("template");
            Append(_templateTag);
        }

        public TurboStreamTag AppendIntoTemplate(HtmlTag htmlTag)
        {
            _templateTag.Append(htmlTag);
            return this;
        }
    }
}