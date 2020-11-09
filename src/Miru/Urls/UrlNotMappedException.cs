using System;

namespace Miru.Urls
{
    public class UrlNotMappedException : MiruException
    {
        public UrlNotMappedException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}