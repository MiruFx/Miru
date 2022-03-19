using Corpo.Skeleton.Features.Categories;
using Miru.PageTesting;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Categories;

public class CategoryNewPageTest : PageTest
{
    [Test]
    public void Can_make_new_category()
    {
        _.Visit(new CategoryNew());

        _.Form<CategoryNew.Command>((f, command) =>
        {
            f.Input(m => m.Name, command.Name);
                
            f.Submit();
        });
            
        _.ShouldHaveText("Category successfully saved");
    }
}