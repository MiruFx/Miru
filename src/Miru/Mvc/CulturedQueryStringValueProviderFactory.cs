using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Miru.Mvc
{
    public class CulturedQueryStringValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var query = context.ActionContext.HttpContext.Request.Query;
            
            if (query.Count > 0)
            {
                var valueProvider = new QueryStringValueProvider(
                    BindingSource.Query,
                    query,
                    CultureInfo.CurrentCulture);

                context.ValueProviders.Add(valueProvider);
            }

            return Task.CompletedTask;
        }
    }
}