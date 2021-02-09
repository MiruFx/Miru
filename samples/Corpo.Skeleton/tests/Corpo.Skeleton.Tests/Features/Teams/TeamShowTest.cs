using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Teams;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Teams
{
    public class ProductShowTest : FeatureTest
    {
        [Test]
        public async Task Can_show_product()
        {
            // arrange
            var product = _.MakeSaving<Team>();
            
            // act
            var response = await _.SendAsync(new TeamShow.Query { Id = product.Id });
            
            // assert
            response.Team.ShouldBe(product);
        }
    }
}