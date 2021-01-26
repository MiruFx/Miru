using System.Collections.Generic;
using System.Threading.Tasks;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class TableCellTagHelperTest : TagHelperTest
    {
        private Corpo.Skeleton.Features.Products.ProductList.Result _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new Corpo.Skeleton.Features.Products.ProductList.Result
            {
                Items = new List<Corpo.Skeleton.Features.Products.ProductList.Item>()
                {
                    new Corpo.Skeleton.Features.Products.ProductList.Item() {Id = 1, Name = "iPhone"},
                    new Corpo.Skeleton.Features.Products.ProductList.Item() {Id = 2, Name = "Samsung"}
                }
            };
        }
        
        [Test]
        public async Task If_no_content_should_add_display()
        {
            // arrange
            var tag = new TdTagHelper
            {
                For = MakeExpression(_viewModel, m => m.Items[0].Name),
                RequestServices = ServiceProvider
            };

            // act
            var output = await ProcessTagAsync(tag, "miru-td");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe("<td><span id=\"Items[0].Name\">iPhone</span></td>");
        }
        
        [Test]
        public async Task If_has_content_should_only_render_td()
        {
            // arrange
            var tag = new TdTagHelper
            {
                For = MakeExpression(_viewModel, m => m.Items[0].Name),
                RequestServices = ServiceProvider
            };

            // act
            var output = await ProcessTagAsync(tag, "miru-td", "Hi");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe("<td>Hi</td>");
        }
        
        [Test]
        public async Task Should_add_attributes_from_conventions()
        {
            // arrange
            var tag = new TdTagHelper
            {
                For = MakeExpression(_viewModel, m => m.Items[0].Id),
                RequestServices = ServiceProvider
            };

            // act
            var output = await ProcessTagAsync(tag, "miru-td");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe("<td class=\"text-right\"><span id=\"Items[0].Id\">1</span></td>");
        }
    }
}