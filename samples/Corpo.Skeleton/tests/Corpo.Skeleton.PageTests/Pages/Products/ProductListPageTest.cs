using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Products;
using Miru;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Products
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