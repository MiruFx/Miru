using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Miru.Mvc
{
    public class MiruObjectResultExecutor : ObjectResultExecutor
    {
        public MiruObjectResultExecutor(
            OutputFormatterSelector formatterSelector, 
            IHttpResponseStreamWriterFactory writerFactory, 
            ILoggerFactory loggerFactory, 
            IOptions<MvcOptions> mvcOptions) : 
                base(formatterSelector, writerFactory, loggerFactory, mvcOptions)
        {
        }

        public override Task ExecuteAsync(ActionContext context, ObjectResult objectResult)
        {
            var ctx = new ObjectResultContext
            {
                Request = context.HttpContext.Request,
                Response = context.HttpContext.Response,
                Model = objectResult.Value,
                ObjectResult = objectResult,
                ActionContext = context
            };

            var config = ctx.GetService<ObjectResultConfiguration>();

            var rules = config.Rules;

            Task task = null;
            
            foreach (var rule in rules)
            {
                if (rule.When(ctx))
                {
                    task = rule.RespondWith(ctx);
                    break;
                }
            }

            if (task != null)
                return task;
            
            return base.ExecuteAsync(context, objectResult);
        }
    }
}