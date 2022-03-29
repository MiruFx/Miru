using System;
using System.ComponentModel.DataAnnotations;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class DisplayTagHelperTest : TagHelperTest
{
    [Test]
    public async Task When_property_is_null_should_render_empty_span()
    {
        // arrange
        var model = new Command { CreatedAt = null };
        var tag = CreateTag(new DisplayTagHelper(), model, m => m.CreatedAt);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-display");
            
        // assert
        html.HtmlShouldBe("<span id=\"CreatedAt\"></span>");
    }
        
    [Test]
    public void Should_render_class()
    {
        // arrange
        var model = new Command { Total = 10 };
        var tag = CreateTag(new DisplayTagHelper(), model, m => m.Total);
            
        // act
        var html = ProcessTag(tag, "miru-display", new { @class = "small" });
            
        // assert
        html.HtmlShouldBe("<span id=\"Total\" class=\"small\">10</span>");
    }
        
    [Test]
    public async Task When_property_is_enum_should_render_display_name()
    {
        // arrange
        var model = new Command { Category = Categories.Movie };
        var tag = CreateTag(new DisplayTagHelper(), model, m => m.Category);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-display");
            
        // assert
        html.HtmlShouldBe("<span id=\"Category\">Filmes</span>");
    }
    
    public class Command
    {
        public DateTime? CreatedAt { get; set; }
        public int Total { get; set; }
        public Categories Category { get; set; }
    }

    public enum Categories
    {
        [Display(Name = "Livros")]
        Book,
        [Display(Name = "Filmes")]
        Movie
    }
}