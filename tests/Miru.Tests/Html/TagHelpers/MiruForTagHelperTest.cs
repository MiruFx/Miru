using System.Collections.Generic;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class MiruForTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task Should_append_class_attribute()
    {
        // arrange
        var model = new Command { Name = "John" };
        var tag = CreateTagWithFor(new InputTagHelper
        {
            AddClass = "added-class"
        }, model, m => m.Name);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-input");
            
        // assert
        html.HtmlShouldBe(
            "<input type=\"text\" value=\"John\" name=\"Name\" id=\"Name\" class=\"added-class\">");            
    }
    
    [Test]
    public async Task Should_overwrite_existing_class_attribute()
    {
        // arrange
        var model = new Command { Name = "John" };
        var tag = CreateTagWithFor(new InputTagHelper() { SetClass = "existent-class" }, model, m => m.Name);
        
        // act
        var html = await ProcessTagAsync(tag, "miru-input", new { @class = "random-class" });
            
        // assert
        html.HtmlShouldBe(
            "<input type=\"text\" value=\"John\" name=\"Name\" id=\"Name\" class=\"existent-class\">");            
    }
    
    [Test]
    public async Task If_tag_already_has_id_attribute_then_it_should_keep_it()
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
        var output = await ProcessTagAsync(tag, "miru-table", new { id = "existent-id" });
            
        // arrange
        output.HtmlShouldContain("<table id=\"existent-id\">");
        output.HtmlShouldContain("</table>");
    }
    
    [Test]
    public async Task If_tag_already_has_name_attribute_then_it_should_keep_it()
    {
        // arrange
        var model = new Command();
        var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.Name);

        // act
        var output = await ProcessTagAsync(tag, "miru-table", new { name = "existent-name" });
            
        // arrange
        output.HtmlShouldContain("<input type=\"text\" value=\"\" name=\"existent-name\" id=\"Name\">");
    }
    
    public class Command
    {
        public string Name { get; set; }
    }
    
    public class Result
    {
        public List<string> Names { get; set; } = new();
    }
}