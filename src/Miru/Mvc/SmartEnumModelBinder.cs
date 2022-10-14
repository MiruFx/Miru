using System;
using System.Reflection;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Miru.Mvc;

public class SmartEnumBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (TypeUtil.IsDerived(context.Metadata.ModelType, typeof(SmartEnum<,>)))
            return new BinderTypeModelBinder(typeof(SmartEnumModelBinder));

        return null;
    }
}

public class SmartEnumModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        Type modelType = bindingContext.ModelMetadata.ModelType;

        if (!TypeUtil.IsDerived(modelType, typeof(SmartEnum<,>)))
            throw new ArgumentException($"{modelType} is not a SmartEnum");

        string propertyName = bindingContext.ModelName;
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(propertyName);
        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(propertyName, valueProviderResult);

        string enumKeyName = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(enumKeyName))
            return Task.CompletedTask;

        // Create smart enum instance from enum key name by calling the FromName static method on the SmartEnum Class
        Type baseSmartEnumType = TypeUtil.GetTypeFromGenericType(modelType, typeof(SmartEnum<,>));
        foreach (MethodInfo methodInfo in baseSmartEnumType.GetMethods())
        {
            if (methodInfo.Name == "FromValue")
            {
                ParameterInfo[] methodsParams = methodInfo.GetParameters();
                if (methodsParams.Length == 1)
                {
                    if (methodsParams[0].ParameterType == typeof(int))
                    {
                        var enumObj = methodInfo.Invoke(null, new object[] { enumKeyName.ToInt() });
                        bindingContext.Result = ModelBindingResult.Success(enumObj);
                        return Task.CompletedTask;
                    }
                }
            }
        }
        bindingContext.ModelState.TryAddModelError(propertyName, $"unable to call FromName on the SmartEnum of type {modelType}");
        return Task.CompletedTask;
    }
}

internal static class TypeUtil
{
    public static bool IsDerived(Type objectType, Type mainType)
    {
        Type currentType = objectType.BaseType;

        if (currentType == null)
        {
            return false;
        }

        while (currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == mainType)
                return true;

            currentType = currentType.BaseType;
        }

        return false;
    }

    public static Type GetTypeFromGenericType(Type objectType, Type mainType)
    {
        Type currentType = objectType.BaseType;

        if (currentType == null)
        {
            return null;
        }

        while (currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == mainType)
                return currentType;

            currentType = currentType.BaseType;
        }

        return null;
    }
}