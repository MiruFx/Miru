using System.Threading.Tasks;
using Corpo.Skeleton.Features.Products;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.Tests.Features.Products
{
    public class ProductCreatedTest : FeatureTest
    {
        [Test]
        public async Task Can_handle_created_product_job()
        {
            // arrange
            var command = _.Make<ProductCreated.Job>();
            
            // act
            await _.SendAsync(command);
            
            // assert
        }
    }
}