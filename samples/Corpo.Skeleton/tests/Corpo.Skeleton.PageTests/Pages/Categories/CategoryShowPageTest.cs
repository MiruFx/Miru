using Corpo.Skeleton.Features.Categories;

namespace Corpo.Skeleton.PageTests.Pages.Categories;

public class CategoryShowPageTest : PageTest
{
    [Test]
    public void Can_show_category()
    {
        var category = _.Make<Category>();
        _.Save(category);
        
        _.Visit(new CategoryShow.Query { Id = category.Id });

        _.ShouldHaveText(category.Name);
    }
}