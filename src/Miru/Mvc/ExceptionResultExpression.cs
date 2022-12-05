using System;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Mvc;

public class ExceptionResultExpression
{
    internal Func<ExceptionResultContext, IActionResult> RespondWith { get; set; }

    public Func<ExceptionResultContext, bool> When { get; set; }

    public void Respond<TActionResult>(Func<ExceptionResultContext, TActionResult> respondWith) where TActionResult : IActionResult
    {
        RespondWith = ctx => respondWith(ctx);
    }
}