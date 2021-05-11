using Miru.Core;
using Miru.Domain;

namespace Miru.Html
{
    public static class ElementNamingExtensions
    {
        public static string FormSummaryId(this ElementNaming naming, object model)
        {
            return $"{ElementNaming.BuildId(Id(model))}-summary";
        }
        
        private static string Id(object model)
        {
            // TODO: automated test
            if (model is IEntity entity)
                return $"{model.GetType().Name.ToKebabCase()}_{entity.Id}";

            var propertyId = model.GetType().GetProperty("Id");
            
            if (propertyId != null)
                return $"{model.GetType().Name.ToKebabCase()}_{propertyId.GetValue(model)}";
            
            return model.GetType().FeatureName().ToKebabCase();
        }
    }
}