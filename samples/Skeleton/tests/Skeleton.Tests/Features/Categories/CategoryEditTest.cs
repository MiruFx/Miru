using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Skeleton.Domain;
using Skeleton.Features.Categories;

namespace Skeleton.Tests.Features.Categories
{
    public class CategoryEditTest : FeatureTest
    {
        [Test]
        public async Task Can_edit_category()
        {
            // arrange
            var category = _.MakeSaving<Category>();
            var command = _.Make<CategoryEdit.Command>(m => m.Id = category.Id);

            // act
            var result = await _.SendAsync(command);

            // assert
            var saved = _.Db(db => db.Categories.First());
            saved.Name.ShouldBe(command.Name);
        }

        public class Validations : ValidationTest<CategoryEdit.Command>
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