using System;

namespace Miru
{
    public class MiruException : Exception
    {
        public MiruException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}