using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Miru.Mvc;
using StackExchange.Exceptional;

namespace Playground.Features.Errors;

public class ErrorList
{
    public class ErrorsController : MiruController
    {
        public ActionResult New() => throw new Exception("Uhhhh new Exception");
        
        [HttpGet("/Errors/All/{path?}/{subPath?}", Name="ErrorLog")]
        public async Task Exceptions() => await ExceptionalMiddleware.HandleRequestAsync(HttpContext);
    }
}