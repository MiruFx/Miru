using System.Collections.Generic;
using System.Threading.Tasks;
using Corpo.Skeleton.Features.Teams;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class TableCellTagHelperTest : TagHelperTest
    {
        private TeamList.Result _model;

        [SetUp]
        public void Setup()
        {
            _model = new TeamList.Result
            {
                Items = new List<TeamList.Item>
                {
                    new() {Id = 1, Name = "iPhone"},
                    new() {Id = 2, Name = "Samsung"}
                }
            };
        }
        
        [Test]
        public async Task If_no_content_should_add_miru_display()
        {
            // arrange
            var tag = new TdTagHelper
            {
                For = MakeExpression(_model, m => m.Items[0].Name),
                RequestServices = ServiceProvider
            };

            // act
            var output = await ProcessTagAsync(tag, "miru-td");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe("<td><span id=\"Items[0].Name\">iPhone</span></td>");
        }
        
        [Test]
        public async Task If_no_content_and_property_is_collection_should_not_render_miru_display()
        {
            // arrange
            var tag = CreateTag(new TdTagHelper(), _model, m => m.Items);

            // act
            var output = await ProcessTagAsync(tag, "miru-td");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe("<td><span id=\"Items\"></span></td>");
        }
        
        [Test]
        public async Task If_has_content_should_only_render_td()
        {
            // arrange
            var tag = new TdTagHelper
            {
                For = MakeExpression(_model, m => m.Items[0].Name),
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
                For = MakeExpression(_model, m => m.Items[0].Id),
                RequestServices = ServiceProvider
            };

            // act
            var output = await ProcessTagAsync(tag, "miru-td");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe("<td><span id=\"Items[0].Id\">1</span></td>");
        }
    }
}