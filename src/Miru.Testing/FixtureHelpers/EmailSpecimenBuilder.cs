using System.Reflection;
using AutoFixture.Kernel;

namespace Miru.Testing.FixtureHelpers
{
    public class EmailSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;

            if (propertyInfo != null)
            {
                if (propertyInfo.Name == "Email" && propertyInfo.PropertyType == typeof(string))
                {
                    // TODO: Fix generate random email
                    //var mailAddress = (MailAddress) context.Resolve(typeof (MailAddress));
                    //return mailAddress.Address;

                    return "aha@oho.com";
                }
            }
            return new NoSpecimen();
        }
    }
}
