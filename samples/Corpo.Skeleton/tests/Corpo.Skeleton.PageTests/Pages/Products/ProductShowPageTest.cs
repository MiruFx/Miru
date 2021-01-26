using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Products;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Products
{
    public class ProductShowPageTest : PageTest
    {
        [Test]
        public void Can_show_product()
        {
            var product = _.MakeSaving<Product>();
            
            _.Visit(new ProductShow.Query { Id = product.Id });

            _.ShouldHaveText(product.Name);
        }
    }
}