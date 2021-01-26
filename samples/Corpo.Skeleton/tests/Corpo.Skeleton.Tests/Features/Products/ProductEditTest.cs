using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Products;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Corpo.Skeleton.Tests.Features.Products
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
            var result = await _.SendAsync(command);

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