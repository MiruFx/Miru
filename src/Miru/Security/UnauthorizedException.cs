using System;

namespace Miru.Security
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = null) : base(message)
        {
        }
    }
}