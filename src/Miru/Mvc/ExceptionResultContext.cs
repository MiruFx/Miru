using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Miru.Userfy;

namespace Miru.Mvc
{
    public class ExceptionResultContext
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public Exception Exception { get; set; }
        public ExceptionContext ExceptionContext { get; set; }
        public IUserSession UserSession { get; set; }
        public IServiceProvider RequestServices { get; set; }

        public T GetService<T>() => RequestServices.GetService<T>();
    }
}