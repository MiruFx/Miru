using System.Collections.Generic;
using Miru.Html.HtmlConfigs;
using Miru.Html.HtmlConfigs.Core;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class TableHeaderTagHelperTest : MiruTagTesting
{
    protected override void HtmlConfig(HtmlConventions htmlConfig)
    {
        htmlConfig.TableHeaders.Always.AddClass("text-muted");
    }

    [Test]
    public void Should_create_header_for_model_property()
    {
        // arrange
        var model = new Result
        {
            Products = new List<Product>
            {
                new() { Name = "iPhone" },
                new() { Name = "Samsung" },
            }
        };
        var tag = TagWithXFor(new TableHeaderTagHelper(), model, x => x.Products[0].Name);

        // act
        var output = ProcessTag(tag, "miru-th");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<th class=\"text-muted\">Name</th>");
    }
    
    [Test]
    public void If_header_has_content_then_should_keep_header_contents()
    {
        // arrange
        var model = new Result
        {
            Products = new List<Product>
            {
                new() { Name = "iPhone" },
                new() { Name = "Samsung" },
            }
        };
        var tag = TagWithXFor(new TableHeaderTagHelper(), model, x => x.Products[0].Name);

        // act
        var output = ProcessTag(tag, "miru-th", content: "Product Name");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<th class=\"text-muted\">Product Name</th>");
    }
    
    [Test]
    public void If_there_is_no_model_then_should_create_empty_header()
    {
        // arrange
        var tag = Tag(new TableHeaderTagHelper());

        // act
        var output = ProcessTag(tag, "miru-th");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<th class=\"text-muted\"></th>");
    }
    
    [Test]
    public void If_html_has_attr_class_then_should_be_appended_to_conventions()
    {
        // arrange
        var tag = Tag(new TableHeaderTagHelper());

        // act
        var output = ProcessTag(tag, "miru-th", new { @class = "pe-5" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<th class=\"text-muted pe-5\"></th>");
    }
    
    [Test]
    public void If_html_has_class_attr_then_should_overwrite_conventions()
    {
        // arrange
        var tag = Tag(new TableHeaderTagHelper());

        // act
        var output = ProcessTag(tag, "miru-th", new { @set_class = "fw-bold" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<th class=\"fw-bold\"></th>");
    }

    public class Result
    {
        public List<Product> Products { get; set; } = new();
    }

    public class Product
    {
        public string Name { get; set; }
    }
}