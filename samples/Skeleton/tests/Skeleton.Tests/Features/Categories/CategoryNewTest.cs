using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Skeleton.Features.Categories;

namespace Skeleton.Tests.Features.Categories
{
    public class CategoryNewTest : FeatureTest
    {
        [Test]
        public async Task Can_make_new_category()
        {
            // arrange
            var command = _.Make<CategoryNew.Command>();
            
            // act
            var result = await _.SendAsync(command);
            
            // assert
            var saved = _.Db(db => db.Categories.First());
            saved.Name.ShouldBe(command.Name);
        }
        
        public class Validations : ValidationTest<CategoryNew.Command>
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