using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Supportreon.Domain;

namespace Supportreon.Tests.Domain
{
    public class ProjectTest : DomainTest
    {
        [Test]
        public void Return_if_project_is_active()
        {
            new Project().IsActive.ShouldBeTrue();
            
            new Project { EndDate = _.Faker().Date.Past() }.IsActive.ShouldBeFalse();
        }
    }
}
