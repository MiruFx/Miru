using System.Collections.Generic;
using Corpo.Skeleton.Features.Teams;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class TableTagHelperTest : TagHelperTest
{
    [Test]
    public void Should_render_table_for_many_items()
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
        var tag = CreateTag(new TableTagHelper(), model, m => m.Teams);

        // act
        var output = ProcessTag(tag, "miru-table");
            
        // arrange
        output.HtmlShouldContain("<table id=\"team-list\">");
        output.HtmlShouldContain("</table>");
    }
        
    [Test]
    public void Should_render_table_for_one_item()
    {
        // arrange
        var model = new TeamList.Result
        {
            Teams = new List<TeamList.TeamView>
            {
                new() {Id = 1, Name = "iPhone"}
            }
        };
        var tag = CreateTag(new TableTagHelper(), model, m => m.Teams);

        // act
        var output = ProcessTag(tag, "miru-table");
            
        // arrange
        output.HtmlShouldContain("<table id=\"team-list\">");
        output.HtmlShouldContain("</table>");
    }
        
    [Test]
    public void Should_not_render_table_for_empty_model()
    {
        // arrange
        var model = new TeamList.Result { Teams = new List<TeamList.TeamView>() };
        var tag = CreateTag(new TableTagHelper(), model, m => m.Teams);

        // act
        var output = ProcessTag(tag, "miru-table");
            
        // arrange
        output.TagName.ShouldBeNullOrEmpty();
        output.Content.IsEmptyOrWhiteSpace.ShouldBeTrue();
    }
}