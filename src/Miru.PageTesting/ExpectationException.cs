using System;

namespace Miru.PageTesting
{
    public class ExpectationException : MiruDriverException
    {
        public ExpectationException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}