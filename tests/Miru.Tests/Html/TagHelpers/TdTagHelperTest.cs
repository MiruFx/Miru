using System.Collections.Generic;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class TdTagHelperTest : MiruTagTesting
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
        var tag = CreateTagWithFor(new TdTagHelper(), _model, m => m.Items[0].Id);

        // act
        var output = await ProcessTagAsync(tag, "miru-td");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe("<td><span id=\"Items[0].Id\">1</span></td>");
    }
    
    [Test]
    public async Task If_property_is_empty_list_then_it_should_render_child_content()
    {
        // arrange
        var model = new TeamList.Result
        {
            Items = new List<TeamList.Item>
            {
                new() {Id = 1, Name = "iPhone", Groups = new()},
            }
        }; 
        var tag = CreateTagWithFor(new TdTagHelper(), model, m => m.Items[0].Groups);

        // act
        var output = await ProcessTagAsync(tag, "miru-td");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe("<td><span id=\"Items[0].Groups\"></span></td>");
    }
    
    [Test]
    public async Task Should_render_empty_td()
    {
        // arrange
        var tag = CreateTag(new TdTagHelper());

        // act
        var output = await ProcessTagAsync(tag, "miru-td");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe("<td></td>");
    }
    
    [Test]
    public async Task If_tag_has_link_attr_then_should_wrap_link_text_into_link_tag()
    {
        // arrange
        var linkToFeature = new TeamList.Query { Id = 99 };
        var tag = CreateTagWithFor(
            new TdTagHelper { LinkFor = linkToFeature, LinkClass = "text-secondary"}, 
            _model, 
            m => m.Items[0].Id);

        // act
        var output = await ProcessTagAsync(tag, "miru-td");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe(
            "<td><span id=\"Items[0].Id\"><a href=\"/TeamList?Id=99\" class=\"text-secondary\">1</a></span></td>");
    }

    public class TeamList
    {
        public class Query
        {
            public long Id { get; set; }
        }

        public class Result
        {
            public IReadOnlyList<Item> Items { get; set; } = new List<Item>();
        }

        public class Item
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public List<Group> Groups { get; set; } = new();
        }

        public class Group
        {
            public string Name { get; set; }
        }
    }
}