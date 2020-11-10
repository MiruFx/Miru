using AutoFixture;
using Miru.Fabrication.FixtureConventions;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Fabrication
{
    public class AutoFakerConventionTest
    {
        private readonly Fixture _fixture;

        public AutoFakerConventionTest()
        {
            _fixture = new Fixture();
            
            _fixture.AddConvention(_ => _.AddAutoFaker());
        }

        [Test]
        public void Should_generate_different_emails()
        {
            var email1 = _fixture.Create<User>().Email;
            var email2 = _fixture.Create<User>().Email;
            
            email1.ShouldNotBe(email2);
        }
        
        [Test]
        public void Should_generate_different_full_names()
        {
            var name1 = _fixture.Create<User>().FullName;
            var name2 = _fixture.Create<User>().FullName;
            
            name1.ShouldNotBe(name2);
        }
        
        public class User
        {
            public long Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
        }
    }
}