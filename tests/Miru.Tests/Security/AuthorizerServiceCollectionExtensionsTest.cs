using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Miru.Security;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Security
{
    public class AuthorizerServiceCollectionExtensionsTest
    {
        private ServiceProvider _sp;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection()
                .AddAuthorizersInAssemblyOf<Query>();

            _sp = services.BuildServiceProvider();
        }
        
        [Test]
        public void Can_get_authorizer_for_a_request()
        {
            _sp.GetServices<IAuthorizer<Query>>().First().ShouldBeOfType<Authorizer>();
        }
        
        [Test]
        public void Return_null_for_request_without_authorizer()
        {
            _sp.GetServices<IAuthorizer<Command>>().ShouldBeEmpty();
        }

        public class Query
        {
        }
        
        public class Command
        {
        }

        public class Authorizer : IAuthorizer<Query>
        {
            public Task<bool> HasAuthorization(Query message)
            {
                return Task.FromResult(true);
            }
        }
    }
}