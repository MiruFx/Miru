using System.IO;
using System.Text;

namespace Miru.Core
{
    public class StringWriter : TextWriter
    {
        private readonly StringBuilder _content = new StringBuilder();

        public override void Write(char value)
        {
            _content.Append(value);
        }

        public override void Write(string value)
        {
            _content.Append(value);
        }

        public override string ToString() => _content.ToString();

        public override Encoding Encoding => Encoding.Unicode;
    }
}