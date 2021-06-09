using System.Collections.Generic;
using System.Linq;
using Corpo.Skeleton.Features.Teams;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class TableTagHelperTest : TagHelperTest
    {
        [Test]
        public void Should_render_table_for_many_items()
        {
            // arrange
            var model = new TeamList.Result
            {
                Items = new List<TeamList.Item>()
                {
                    new() {Id = 1, Name = "iPhone"},
                    new() {Id = 2, Name = "Samsung"}
                }
            };
            var tag = CreateTag(new TableTagHelper(), model, m => m.Items);

            // act
            var output = ProcessTag(tag, "miru-table");
            
            // arrange
            output.TagName.ShouldBe("table");
            output.Attributes.Single(m => m.Name == "id").Value.ShouldBe("team-list");
        }
        
        [Test]
        public void Should_render_table_for_one_item()
        {
            // arrange
            var model = new TeamList.Result
            {
                Items = new List<TeamList.Item>
                {
                    new() {Id = 1, Name = "iPhone"}
                }
            };
            var tag = CreateTag(new TableTagHelper(), model, m => m.Items);

            // act
            var output = ProcessTag(tag, "miru-table");
            
            // arrange
            output.TagName.ShouldBe("table");
            output.Attributes.Single(m => m.Name == "id").Value.ShouldBe("team-list");
        }
        
        [Test]
        public void Should_not_render_table_for_empty_model()
        {
            // arrange
            var model = new TeamList.Result { Items = new List<TeamList.Item>() };
            var tag = CreateTag(new TableTagHelper(), model, m => m.Items);

            // act
            var output = ProcessTag(tag, "miru-table");
            
            // arrange
            output.TagName.ShouldBeNullOrEmpty();
            output.Content.IsEmptyOrWhiteSpace.ShouldBeTrue();
        }
    }
}