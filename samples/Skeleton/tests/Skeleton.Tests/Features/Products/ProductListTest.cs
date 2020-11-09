using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Domain;
using Skeleton.Features.Products;

namespace Skeleton.Tests.Features.Products
{
    public class ProductListTest : FeatureTest
    {
        [Test]
        public async Task Can_list_products()
        {
            // arrange
            var products = _.MakeManySaving<Product>();
            
            // act
            var result = await _.Send(new ProductList.Query());
            
            // assert
            result.Items.ShouldCount(products.Count());
        }
    }
}