using Microsoft.AspNetCore.Mvc;

namespace Miru.Security;

public class UnauthorizedException : Exception
{
    public ActionResult ActionResult { get; }

    public UnauthorizedException(
        string message = null, 
        ActionResult actionResult = null) : base(message)
    {
        ActionResult = actionResult;
    }
}