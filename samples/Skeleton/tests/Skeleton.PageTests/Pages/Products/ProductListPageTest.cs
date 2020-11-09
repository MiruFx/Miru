using Miru;
using Miru.PageTesting;
using NUnit.Framework;
using Skeleton.Features.Products;
using Miru.Testing;
using Skeleton.Domain;

namespace Skeleton.PageTests.Pages.Products
{
    public class ProductListPageTest : PageTest
    {
        [Test]
        public void Can_list_products()
        {
            var products = _.MakeManySaving<Product>();
            
            _.Visit<ProductList>();
            
            _.ShouldHaveText("Products");

            _.Display<ProductList.Result>(x =>
            {
                x.ShouldHave(m => m.Items[0].Name, products.At(0).Name);
            });
        }
    }
}