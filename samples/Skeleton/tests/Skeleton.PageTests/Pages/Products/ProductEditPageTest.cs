using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Domain;
using Skeleton.Features.Products;

namespace Skeleton.PageTests.Pages.Products
{
    public class ProductEditPageTest : PageTest
    {
        [Test]
        public void Can_edit_product()
        {
            var product = _.MakeSaving<Product>();
            
            _.Visit(new ProductEdit.Query { Id = product.Id });

            _.Form<ProductEdit.Command>((f, command) =>
            {
                f.Input(m => m.Name, command.Name);
                
                f.Submit();
            });
            
            _.ShouldHaveText("Product successfully saved");
        }
    }
}