using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Miru.Userfy;

namespace Miru.Mvc;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ExceptionResultConfiguration _exceptionResultConfig;

    public ExceptionFilter(ExceptionResultConfiguration exceptionResultConfig)
    {
        _exceptionResultConfig = exceptionResultConfig;
    }

    public void OnException(ExceptionContext context)
    {
        var ctx = new ExceptionResultContext
        {
            Exception = context.Exception,
            ExceptionContext = context,
            Request = context.HttpContext.Request,
            Response = context.HttpContext.Response,
            RequestServices = context.HttpContext.RequestServices,
            UserSession = context.HttpContext.RequestServices.GetService<IUserSession>()
        };

        var rules = _exceptionResultConfig.Rules;

        IActionResult result = null;
            
        foreach (var rule in rules)
        {
            if (rule.When(ctx))
            {
                result = rule.RespondWith(ctx);
                break;
            }
        }

        if (result != null)
        {
            App.Framework.Error(ctx.Exception, "Exception handled from the application");

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}