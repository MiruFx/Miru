using Corpo.Skeleton.Features.Categories;

namespace Corpo.Skeleton.Tests.Features.Categories;

public class CategoryShowTest : FeatureTest
{
    [Test]
    public async Task Can_show_categories()
    {
        // arrange
        var category = _.Make<Category>();
        await _.SaveAsync(category);
            
        // act
        var response = await _.SendAsync(new CategoryShow.Query { Id = category.Id });
            
        // assert
        response.Category.ShouldBe(category);
    }
}