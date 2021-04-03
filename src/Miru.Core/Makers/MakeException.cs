using System;

namespace Miru.Core.Makers
{
    public class MakeException : Exception
    {
        public MakeException(string message) : base(message)
        {
        }
    }
}