using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Features.Products;

namespace Skeleton.Tests.Features.Products
{
    public class ProductCreatedTest : FeatureTest
    {
        [Test]
        public async Task Can_handle_created_product_job()
        {
            // arrange
            var command = _.Make<ProductCreated.Job>();
            
            // act
            await _.Send(command);
            
            // assert
        }
    }
}