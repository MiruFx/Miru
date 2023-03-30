using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Miru.Mvc;

public class ObjectResultConfigExpression
{
    public void Respond<TActionResult>(Func<ObjectResultContext, TActionResult> respondWith) where TActionResult : IActionResult
    {
        RespondWith = ctx =>
        {
            var executor = ctx.GetService<IActionResultExecutor<TActionResult>>();

            if (executor == null)
                throw new InvalidOperationException(
                    $@"Could not find a IActionResultExecutor<{typeof(TActionResult).Name}>

Register one in your Startup.cs");
                
            return executor.ExecuteAsync(ctx.ActionContext, respondWith(ctx));
        };
    }

    public Func<ObjectResultContext, Task> RespondWith { get; set; }

    public Func<ObjectResultContext, bool> When { get; set; }
}