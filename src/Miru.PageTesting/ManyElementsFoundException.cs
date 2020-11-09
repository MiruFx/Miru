using System;

namespace Miru.PageTesting
{
    public class ManyElementsFoundException : MiruDriverException
    {
        public ManyElementsFoundException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}