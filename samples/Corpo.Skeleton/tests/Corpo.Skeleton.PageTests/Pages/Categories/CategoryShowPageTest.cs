using Corpo.Skeleton.Domain;
using Corpo.Skeleton.Features.Categories;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;

namespace Corpo.Skeleton.PageTests.Pages.Categories
{
    public class CategoryShowPageTest : PageTest
    {
        [Test]
        public void Can_show_category()
        {
            var category = _.MakeSaving<Category>();
            
            _.Visit(new CategoryShow.Query { Id = category.Id });

            _.ShouldHaveText(category.Name);
        }
    }
}