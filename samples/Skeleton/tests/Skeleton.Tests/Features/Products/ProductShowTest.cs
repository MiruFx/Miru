using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Skeleton.Domain;
using Skeleton.Features.Products;

namespace Skeleton.Tests.Features.Products
{
    public class ProductShowTest : FeatureTest
    {
        [Test]
        public async Task Can_show_product()
        {
            // arrange
            var product = _.MakeSaving<Product>();
            
            // act
            var response = await _.Send(new ProductShow.Query { Id = product.Id });
            
            // assert
            response.Product.ShouldBe(product);
        }
    }
}