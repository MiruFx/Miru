using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Miru.Domain;

namespace Miru.Mvc
{
    public class EnumerationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsComplexType
                && (context.Metadata.ModelType.ImplementsGenericOf(typeof(Enumeration<,>)) 
                    || context.Metadata.ModelType.ImplementsGenericOf(typeof(Enumeration<>))))
            {
                return new EnumerationModelBinder();
            }

            return null;
        }
    }
}