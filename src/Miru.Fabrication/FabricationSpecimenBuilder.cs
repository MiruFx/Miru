using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Miru.Domain;
using Miru.Fabrication.FixtureConventions;

namespace Miru.Fabrication
{
    public class FabricationSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Fixture _fixture;
        private readonly FabricatedSession _session;

        public FabricationSpecimenBuilder(Fixture fixture, FabricatedSession session)
        {
            _fixture = fixture;
            _session = session;
        }

        private bool ShouldReturnNoSpecimen(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsPrimitive || 
                   propertyInfo.PropertyType.IsValueType ||
                   propertyInfo.PropertyType == typeof(string);
        }
        
        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;
            var typeInfo = request as TypeInfo;
            
            if (propertyInfo == null || ShouldReturnNoSpecimen(propertyInfo))
                return new NoSpecimen();

            var propertyIsEntity = propertyInfo.PropertyType.Implements<IEntity>();

            if (propertyIsEntity)
            {
                var singleton = _session.GetSingleton(propertyInfo.PropertyType);
                
                if (singleton != null)
                    return singleton;
            }
            
            var instance = _fixture.CreateByType(propertyInfo.PropertyType);

            // if (instance is OmitSpecimen)
            //     return new NoSpecimen();
            
            if (propertyIsEntity && instance != null && instance is not OmitSpecimen)
                _session.AddSingleton(propertyInfo.PropertyType, instance);
            
            return instance;
        }
    }
}