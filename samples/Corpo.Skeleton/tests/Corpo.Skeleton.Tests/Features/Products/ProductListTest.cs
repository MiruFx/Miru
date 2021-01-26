using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Products;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.Tests.Features.Products
{
    public class ProductListTest : FeatureTest
    {
        [Test]
        public async Task Can_list_products()
        {
            // arrange
            var products = _.MakeManySaving<Product>();
            
            // act
            var result = await _.SendAsync(new ProductList.Query());
            
            // assert
            result.Items.ShouldCount(products.Count());
        }
    }
}