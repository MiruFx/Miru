using System.Collections.Generic;
using System.Linq;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class TableTagHelperTest : TagHelperTest
    {
        [Test]
        public void Should_render_table()
        {
            // arrange
            var viewModel = new Skeleton.Features.Products.ProductList.Result
            {
                Items = new List<Skeleton.Features.Products.ProductList.Item>()
                {
                    new Skeleton.Features.Products.ProductList.Item() {Id = 1, Name = "iPhone"},
                    new Skeleton.Features.Products.ProductList.Item() {Id = 2, Name = "Samsung"}
                }
            };

            var tag = new TableTagHelper
            {
                For = MakeExpression(viewModel, "Items", viewModel.Items),
                RequestServices = ServiceProvider
            };

            // act
            var output = ProcessTag(tag, "miru-table");
            
            // arrange
            output.TagName.ShouldBe("table");
            output.Attributes.Single(m => m.Name == "id").Value.ShouldBe("product-list");
        }
    }
}