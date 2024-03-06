using Miru.Fabrication;

namespace Miru.Testing
{
    public static class CustomFabricatorExtensions
    {
        public static IEnumerable<TModel> MakeMany<TModel>(
            this ICustomFabricator<TModel> fabricator, int count, Action<TModel> action) where TModel : class
        {
            var allMade = fabricator.MakeMany(count);

            foreach (var made in allMade)
                action(made);
            
            return allMade;
        }
    }
}