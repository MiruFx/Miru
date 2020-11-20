using System.Linq;
using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Domain;
using Skeleton.Features.Categories;
using Skeleton.Features.Products;

namespace Skeleton.Tests.Features.Categories
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