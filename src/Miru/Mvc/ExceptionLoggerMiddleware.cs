using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Miru.Mvc;

public class ExceptionLoggerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            App.Framework.Error(exception, "Exception thrown in the application");
            throw;
        }
    }
}