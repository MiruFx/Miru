using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Skeleton.Features.Products;

namespace Skeleton.Tests.Features.Products
{
    public class ProductNewTest : FeatureTest
    {
        [Test]
        public async Task Can_make_new_product()
        {
            // arrange
            var command = _.Make<ProductNew.Command>();
            
            // act
            var result = await _.SendAsync(command);
            
            // assert
            var saved = _.Db(db => db.Products.First());
            saved.Name.ShouldBe(command.Name);
        }
        
        public class Validations : ValidationTest<ProductNew.Command>
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