using Corpo.Skeleton.Features.Products;
using Miru.PageTesting;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Products
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