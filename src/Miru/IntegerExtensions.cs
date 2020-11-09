using System;
using System.Text;

namespace Miru
{
    public static class IntegerExtensions
    {
        public static string Times(this int value, Func<string> func)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < value; i++)
            {
                ret.Append(func());
            }

            return ret.ToString();
        }
        
        public static string Times(this int value, Func<int, string> func)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < value; i++)
            {
                ret.Append(func(i));
            }

            return ret.ToString();
        }
    }
}
