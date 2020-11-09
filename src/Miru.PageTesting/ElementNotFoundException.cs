using System;

namespace Miru.PageTesting
{
    public class ElementNotFoundException : MiruDriverException
    {
        public ElementNotFoundException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}