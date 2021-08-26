using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Miru.Mvc;
using Miru.Mvc;

namespace Miru.Tests.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static void UseEnumerationModelBinding(this MvcOptions opts)
        {
            var binderToFind = opts.ModelBinderProviders
                .FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));

            if (binderToFind == null) 
                return;

            var index = opts.ModelBinderProviders.IndexOf(binderToFind);
            
            opts.ModelBinderProviders.Insert(index, new EnumerationModelBinderProvider());
        }
    }
}