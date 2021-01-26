using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Products;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Products
{
    public class ProductShowTest : FeatureTest
    {
        [Test]
        public async Task Can_show_product()
        {
            // arrange
            var product = _.MakeSaving<Product>();
            
            // act
            var response = await _.SendAsync(new ProductShow.Query { Id = product.Id });
            
            // assert
            response.Product.ShouldBe(product);
        }
    }
}