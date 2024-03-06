using AutoFixture.Kernel;

namespace Miru.Testing.FixtureHelpers
{
    public class IsDeletedBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var property = request as PropertyInfo;

            if (property == null)
            {
                return new NoSpecimen();
            }

            if (property.Name.Equals("IsDeleted"))
            {
                return false;
            }

            return new NoSpecimen();
        }
    }
}
