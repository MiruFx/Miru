using System.Linq;
using System.Threading.Tasks;
using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Categories;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.Tests.Features.Categories
{
    public class CategoryListTest : FeatureTest
    {
        [Test]
        public async Task Can_list_categories()
        {
            // arrange
            var categories = _.MakeManySaving<Category>();
            
            // act
            var result = await _.SendAsync(new CategoryList.Query());
            
            // assert
            result.Items.ShouldCount(categories.Count());
        }
    }
}