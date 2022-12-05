using System.Collections.Generic;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class TableCellTagHelperTest : MiruTagTesting
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
    public void If_no_content_should_add_miru_display()
    {
        // arrange
        var tag = TagWithFor(new TableCellTagHelper(), _model, m => m.Items[0].Name);

        // act
        var output = ProcessTag(tag, "miru-td");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<td><span>iPhone</span></td>");
    }
        
    // [Test]
    // public void If_no_content_and_property_is_collection_should_not_render_miru_display()
    // {
    //     // arrange
    //     var tag = TagWithFor(new TableCellTagHelper2(), _model, m => m.Items);
    //
    //     // act
    //     var output = ProcessTag(tag, "miru-td");
    //         
    //     // assert
    //     output.HtmlShouldBe("<td></td>");
    // }
        
    [Test]
    public void If_tag_has_content_then_should_render_td_with_existing_content()
    {
        // arrange
        var tag = TagWithFor(new TableCellTagHelper(), _model, m => m.Items[0].Name);

        // act
        var output = ProcessTag(tag, "miru-td", content: "Hi");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<td>Hi</td>");
    }
        
    [Test]
    public void Should_add_attributes_from_conventions()
    {
        // arrange
        var tag = TagWithFor(new TableCellTagHelper(), _model, m => m.Items[0].Id);

        // act
        var output = ProcessTag(tag, "miru-td");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<td><span>1</span></td>");
    }
    
    [Test]
    public void If_property_is_empty_list_then_it_should_render_child_content()
    {
        // arrange
        var model = new TeamList.Result
        {
            Items = new List<TeamList.Item>
            {
                new() {Id = 1, Name = "iPhone", Groups = new()},
            }
        }; 
        var tag = TagWithFor(new TableCellTagHelper(), model, m => m.Items[0].Groups);

        // act
        var output = ProcessTag(tag, "miru-td");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<td></td>");
    }
    
    [Test]
    public void Should_render_empty_td()
    {
        // arrange
        var tag = Tag(new TableCellTagHelper());

        // act
        var output = ProcessTag(tag, "miru-td");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<td><span></span></td>");
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