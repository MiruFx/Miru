using System.Collections.Generic;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class TableTagHelperTest : MiruTagTesting
{
    [Test]
    public void If_model_has_items_then_should_render_table()
    {
        // arrange
        var model = new TeamList.Result
        {
            Teams = new List<TeamList.TeamView>()
            {
                new() {Id = 1, Name = "iPhone"},
                new() {Id = 2, Name = "Samsung"}
            }
        };
        var tag = TagWithXFor(new TableTagHelper(), model, x => x.Teams);

        // act
        var output = ProcessTag(tag, "miru-table2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<table></table>");
    }
        
    [Test]
    public void If_model_has_one_item_then_should_render_table()
    {
        // arrange
        var model = new TeamList.Result
        {
            Teams = new List<TeamList.TeamView>
            {
                new() {Id = 1, Name = "iPhone"}
            }
        };
        var tag = TagWithXFor(new TableTagHelper(), model, x => x.Teams);

        // act
        var output = ProcessTag(tag, "miru-table2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<table></table>");
    }
        
    [Test]
    public void If_model_is_empty_then_should_not_render_table()
    {
        // arrange
        var model = new TeamList.Result
        {
            Teams = new List<TeamList.TeamView>()
        };
        var tag = TagWithXFor(new TableTagHelper(), model, x => x.Teams);

        // act
        var output = ProcessTag(tag, "miru-table2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, string.Empty);
    }
    
    [Test]
    public void Should_render_table_with_no_for_or_model()
    {
        // arrange
        var tag = Tag<TableTagHelper>();
    
        // act
        var output = ProcessTag(tag, "miru-table2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<table></table>");
    }
    
    public class TeamList
    {
        public class Result
        {
            public IReadOnlyList<TeamView> Teams { get; set; } = new List<TeamView>();
        }

        public class TeamView
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
    }
}