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
        public void Should_render_table()
        {
            // arrange
            var viewModel = new TeamList.Result
            {
                Items = new List<TeamList.Item>()
                {
                    new TeamList.Item() {Id = 1, Name = "iPhone"},
                    new TeamList.Item() {Id = 2, Name = "Samsung"}
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
            output.Attributes.Single(m => m.Name == "id").Value.ShouldBe("team-list");
        }
    }
}