using System;

namespace Miru.Core
{
    public class Console2
    {
        public static void GreyLine(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }
        
        public static void YellowLine(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }

        public static void Yellow(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message, args);
            Console.ResetColor();
        }

        public static void RedLine(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }

        public static void Line(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
        
        public static void Write(string message, params object[] args)
        {
            Console.Write(message, args);
        }

        public static void White(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message, args);
            Console.ResetColor();
        }
        
        public static void WhiteLine(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }
        
        public static void WhiteLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void GreenLine(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }
        
        public static void Green(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message, args);
            Console.ResetColor();
        }

        public static void BreakLine()
        {
            Console.WriteLine();
        }
        
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
    }
}