using System;

namespace Miru.PageTesting
{
    public class PageTestException : Exception
    {
        public PageTestException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}