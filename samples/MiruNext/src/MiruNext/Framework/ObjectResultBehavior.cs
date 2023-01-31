using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Miru;
using Miru.Mvc;
using MiruNext.Features.Orders;

namespace MiruNext.Framework;

public class ObjectResultBehavior : IGlobalPostProcessor
{
    public async Task PostProcessAsync(
        object req, 
        object res, 
        HttpContext ctx, 
        IReadOnlyCollection<ValidationFailure> failures,
        CancellationToken ct)
    {
        if (ctx.Request.CanAccept("text/html"))
        {
        }
        
        // if (ctx.Request.CanAccept("text/html"))
        // {
        //     var renderer = ctx.RequestServices.GetRequiredService<ViewRenderService>();
        //
        //     var viewName = res.GetAttribute<ViewAttribute>().ViewName;
        //     
        //     await renderer.RenderAsync(viewName, res, ctx);
        // }
    }
}