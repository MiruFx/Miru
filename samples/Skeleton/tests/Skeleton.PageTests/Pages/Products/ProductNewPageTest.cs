using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Domain;
using Skeleton.Features.Products;

namespace Skeleton.PageTests.Pages.Products
{
    public class ProductNewPageTest : PageTest
    {
        [Test]
        public void Can_make_new_product()
        {
            _.Visit(new ProductNew());

            _.Form<ProductNew.Command>((f, command) =>
            {
                f.Input(m => m.Name, command.Name);
                
                f.Submit();
            });
            
            _.ShouldHaveText("Product successfully saved");
        }
    }
}