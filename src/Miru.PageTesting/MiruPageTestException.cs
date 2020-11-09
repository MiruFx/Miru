using System;

namespace Miru.PageTesting
{
    public class MiruPageTestException : Exception
    {
        public MiruPageTestException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}