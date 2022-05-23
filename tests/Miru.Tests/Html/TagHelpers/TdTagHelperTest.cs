using System.Collections.Generic;
using System.Threading.Tasks;
using Corpo.Skeleton.Features.Teams;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers;

public class TdTagHelperTest : TagHelperTest
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
        var tag = CreateTagWithFor(new TdTagHelper(), _model, m => m.Items[0].Name);

        // act
        var output = await ProcessTagAsync(tag, "miru-td");
            
        // assert
        output.HtmlShouldBe("<td><span id=\"Items[0].Name\">iPhone</span></td>");
    }
        
    [Test]
    public async Task If_no_content_and_property_is_collection_should_not_render_miru_display()
    {
        // arrange
        var tag = CreateTagWithFor(new TdTagHelper(), _model, m => m.Items);

        // act
        var output = await ProcessTagAsync(tag, "miru-td");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe("<td><span id=\"Items\"></span></td>");
    }
        
    [Test]
    public async Task If_tag_has_content_then_should_render_td_with_existing_content()
    {
        // arrange
        var tag = CreateTagWithFor(new TdTagHelper(), _model, m => m.Items[0].Name);

        // act
        var output = await ProcessTagAsync(tag, "miru-td", "Hi");
            
        // assert
        output.HtmlShouldBe("<td>Hi</td>");
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

    public class TeamList
    {
        public class Query
        {
        }

        public class Result
        {
            public IReadOnlyList<Item> Items { get; set; } = new List<Item>();
        }

        public class Item
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
    }
}