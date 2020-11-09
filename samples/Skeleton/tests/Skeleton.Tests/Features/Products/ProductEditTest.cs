using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Features.Products;
using Shouldly;
using System.Linq;
using Skeleton.Domain;

namespace Skeleton.Tests.Features.Products
{
    public class ProductEditTest : FeatureTest
    {
        [Test]
        public async Task Can_edit_product()
        {
            // arrange
            var product = _.MakeSaving<Product>();
            var command = _.Make<ProductEdit.Command>(m => m.Id = product.Id);

            // act
            var result = await _.Send(command);

            // assert
            var saved = _.Db(db => db.Products.First());
            saved.Name.ShouldBe(command.Name);
        }

        public class Validations : ValidationTest<ProductEdit.Command>
        {
            [Test]
            public void Name_is_required()
            {
                ShouldBeValid(m => m.Name, Request.Name);
            
                ShouldBeInvalid(m => m.Name, string.Empty);
            }
        }
    }
}