using System;

namespace Miru.PageTesting
{
    public class MiruDriverException : Exception
    {
        protected MiruDriverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}