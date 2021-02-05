using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HtmlTags.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Mvc
{
    public abstract class MiruController : Controller
    {
        protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return await ControllerContext.HttpContext.RequestServices.GetService<IMediator>().Send(request);
        }
        
        /// <summary>
        /// Send request to MediatR exposing the response's properties into the ViewBag
        /// </summary>
        protected async Task<TResponse> SendAsync<TResponse, TProperty>(
            IRequest<TResponse> request,
            params Expression<Func<TResponse, TProperty>>[] viewBagsProperties)
        {
            var response = await SendAsync(request);
            
            foreach (var viewBagsProperty in viewBagsProperties)
            {
                // FIXME: use .ToAccessor from other lib not from HtmlTags
                var accessor = viewBagsProperty.ToAccessor();
                var name = accessor.InnerProperty.Name;
                var instance = accessor.GetValue(response);
                ViewData[name] = instance;
            }

            return response;
        }
    }
}
