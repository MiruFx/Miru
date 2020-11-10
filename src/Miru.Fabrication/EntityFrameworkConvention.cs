using System.Collections.ObjectModel;
using Miru.Fabrication.FixtureConventions;

namespace Miru.Fabrication
{
    public static class EntityFrameworkConvention
    {
        public static ConventionExpression AddEntityFramework(this ConventionExpression cfg)
        {
            cfg.IfProperty(p => p.Name.EndsWith("Id")).Ignore();

            cfg.IfProperty(p => p.PropertyType.Implements(typeof(Collection<>)) || 
                                  p.PropertyType.ImplementsGenericOf(typeof(Collection<>))).Ignore();
            
            return cfg;
        }
    }
}