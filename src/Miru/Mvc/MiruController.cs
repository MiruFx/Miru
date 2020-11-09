using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Mvc
{
    public abstract class MiruController : Controller
    {
        protected async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            return await ControllerContext.HttpContext.RequestServices.GetService<IMediator>().Send(request);
        }
    }
}
