using Miru;
using Miru.PageTesting;
using Miru.Testing;
using NUnit.Framework;
using Skeleton.Domain;
using Skeleton.Features.Categories;

namespace Skeleton.PageTests.Pages.Categories
{
    public class CategoryListPageTest : PageTest
    {
        [Test]
        public void Can_list_categories()
        {
            var categories = _.MakeManySaving<Category>();
            
            _.Visit<CategoryList>();
            
            _.ShouldHaveText("Category");

            _.Display<CategoryList.Result>(x =>
            {
                x.ShouldHave(m => m.Items[0].Name, categories.At(0).Name);
            });
        }
    }
}