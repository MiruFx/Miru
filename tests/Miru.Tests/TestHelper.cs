using System;
using System.IO;
using System.Text;

namespace Miru.Tests;

public static class TestHelper
{
    public static string ReadingConsoleOutput(Action action)
    {
        var defaultWriter = Console.Out;
        var writer = new StringWriter();
            
        Console.SetOut(writer);
            
        action();
            
        var output = writer.ToString();
        Console.SetOut(defaultWriter);

        return output;
    }
    
    public class StringWriter : TextWriter
    {
        private readonly StringBuilder _content = new();

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