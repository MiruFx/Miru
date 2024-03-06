using Corpo.Skeleton.Features.Categories;

namespace Corpo.Skeleton.PageTests.Pages.Categories;

public class CategoryListPageTest : PageTest
{
    [Test]
    public void Can_list_categories()
    {
        var categories = _.MakeMany<Category>();
        _.Save(categories);
        
        _.Visit<CategoryList>();
            
        _.ShouldHaveText("Category");

        _.Display<CategoryList.Result>(x =>
        {
            x.ShouldHave(m => m.Items[0].Name, categories.At(0).Name);
        });
    }
}