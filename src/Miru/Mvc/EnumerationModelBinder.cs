using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Miru.Domain;

namespace Miru.Mvc
{
    public class EnumerationModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // _logger.AttemptingToBindModel(bindingContext);

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                // _logger.FoundNoValueInRequest(bindingContext);

                // no entry
                // _logger.DoneAttemptingToBindModel(bindingContext);
                return Task.CompletedTask;
            }

            var modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            var metadata = bindingContext.ModelMetadata;
            var type = metadata.UnderlyingOrModelType;
            try
            {
                var value = valueProviderResult.FirstValue;

                object model = null;
                if (string.IsNullOrWhiteSpace(value))
                {
                    // Parse() method trims the value (with common DateTimeSyles) then throws if the result is empty.
                    model = null;
                }
                else if (type.ImplementsGenericOf(typeof(Enumeration<>)) && int.TryParse(value, out int id))
                {
                    model = typeof(Enumeration<,>)
                        .MakeGenericType(type, typeof(int))
                        .GetMethod("FromValue")?
                        .Invoke(null, new object[] { id });
                }
                else if (type.ImplementsGenericOf(typeof(Enumeration<,>)))
                {
                    model = typeof(Enumeration<,>)
                        .MakeGenericType(type, type.BaseType?.GetGenericArguments()[1])
                        .GetMethod("FromValue")?
                        .Invoke(null, new object[] { value });

                    // model = DateTime.Parse(value, valueProviderResult.Culture);
                }
                else
                {
                    throw new NotSupportedException();
                }

                // When converting value, a null model may indicate a failed conversion for an otherwise required
                // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
                // current bindingContext. If not, an error is logged.
                if (model == null && !metadata.IsReferenceOrNullableType)
                {
                    modelState.TryAddModelError(
                        modelName,
                        metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                            valueProviderResult.ToString()));
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
            catch (Exception exception)
            {
                // Conversion failed.
                modelState.TryAddModelError(modelName, exception, metadata);
            }

            // _logger.DoneAttemptingToBindModel(bindingContext);
            return Task.CompletedTask;
        }
    }
}