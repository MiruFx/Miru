using Corpo.Skeleton.Features.Categories;

namespace Corpo.Skeleton.PageTests.Pages.Categories;

public class CategoryEditPageTest : PageTest
{
    [Test]
    public void Can_edit_category()
    {
        var category = _.MakeSaving<Category>();
            
        _.Visit(new CategoryEdit.Query { Id = category.Id });

        _.Form<CategoryEdit.Command>((f, command) =>
        {
            f.Input(m => m.Name, command.Name);
                
            f.Submit();
        });
            
        _.ShouldHaveText("Category successfully saved");
    }
}