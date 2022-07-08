using System.Collections.Generic;
using Miru.Html;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class TableTagHelperTest : MiruTagTesting
{
    protected override void HtmlConfiguration(HtmlConfiguration htmlConfig)
    {
        htmlConfig.Tables.Always.AddClass("table");
    }

    [Test]
    public async Task Should_render_table_for_many_items()
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
        var tag = CreateTagWithFor(new TableTagHelper(), model, m => m.Teams);

        // act
        var output = await ProcessTagAsync(tag, "miru-table");
            
        // arrange
        output.HtmlShouldContain("<table id=\"team-list-table\" class=\"table\">");
        output.HtmlShouldContain("</table>");
    }
        
    [Test]
    public async Task Should_render_table_for_one_item()
    {
        // arrange
        var model = new TeamList.Result
        {
            Teams = new List<TeamList.TeamView>
            {
                new() {Id = 1, Name = "iPhone"}
            }
        };
        var tag = CreateTagWithFor(new TableTagHelper(), model, m => m.Teams);

        // act
        var output = await ProcessTagAsync(tag, "miru-table");
            
        // arrange
        output.HtmlShouldContain("<table id=\"team-list-table\" class=\"table\">");
        output.HtmlShouldContain("</table>");
    }
        
    [Test]
    public async Task Should_not_render_table_for_empty_model()
    {
        // arrange
        var model = new TeamList.Result { Teams = new List<TeamList.TeamView>() };
        var tag = CreateTagWithFor(new TableTagHelper(), model, m => m.Teams);

        // act
        var output = await ProcessTagAsync(tag, "miru-table");
            
        // arrange
        output.TagName.ShouldBeNullOrEmpty();
        output.Content.IsEmptyOrWhiteSpace.ShouldBeTrue();
    }
    
    [Test]
    public async Task Should_render_table_with_no_for_or_model()
    {
        // arrange
        var tag = CreateTag(new TableTagHelper());

        // act
        var output = await ProcessTagAsync(tag, "miru-table");
            
        // arrange
        output.HtmlShouldContain("<table class=\"table\">");
        output.HtmlShouldContain("</table>");
    }
    
    [Test]
    public async Task Should_render_table_with_model_attribute()
    {
        // arrange
        var model = new TeamList.Result
        {
            Teams = new List<TeamList.TeamView>
            {
                new() {Id = 1, Name = "iPhone"},
                new() {Id = 2, Name = "Samsung"}
            }
        };
        var tag = CreateTagWithModel(new TableTagHelper(), model);
        
        // act
        var output = await ProcessTagAsync(tag, "miru-table");
            
        // arrange
        output.HtmlShouldContain("<table class=\"table\">");
        output.HtmlShouldContain("</table>");
    }
}