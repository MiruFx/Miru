using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using Skeleton.Domain;
using Skeleton.Features.Categories;

namespace Skeleton.Tests.Features.Categories
{
    public class CategoryShowTest : FeatureTest
    {
        [Test]
        public async Task Can_show_categories()
        {
            // arrange
            var category = _.MakeSaving<Category>();
            
            // act
            var response = await _.SendAsync(new CategoryShow.Query { Id = category.Id });
            
            // assert
            response.Category.ShouldBe(category);
        }
    }
}