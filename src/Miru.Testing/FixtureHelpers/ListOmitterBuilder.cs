using AutoFixture.Kernel;

namespace Miru.Testing.FixtureHelpers
{
    public class ListOmitterBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi != null)
            {
                var isIEnumerable = pi.PropertyType
                    .GetInterfaces()
                    .Any(t => t.IsGenericType && (
                        t.GetGenericTypeDefinition() == typeof(IList<>) ||
                        t.GetGenericTypeDefinition() == typeof(ICollection<>)));

                if (isIEnumerable)
                {
                    if (pi.PropertyType.IsArray)
                    {
                        return Array.CreateInstance(pi.PropertyType.GetElementType(), 0);
                    }

                    var genericArguments = pi.PropertyType.GetGenericArguments();
                    var concreteType = typeof(List<>).MakeGenericType(genericArguments);
                    var instance = Activator.CreateInstance(concreteType);
                    return instance;
                }
            }

            return new NoSpecimen();
        }
    }
}
