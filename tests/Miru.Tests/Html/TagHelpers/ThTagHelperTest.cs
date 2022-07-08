using System.Collections.Generic;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class ThTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task Should_create_header_for_model_property()
    {
        // arrange
        var model = new Result()
        {
            Products = new List<Product>()
            {
                new() { Name = "iPhone" },
                new() { Name = "Samsung" },
            }
        };
        var tag = CreateTagWithFor(new ThTagHelper(), model, x => x.Products[0].Name);

        // act
        var html = await ProcessTagAsync(tag, "miru-th");
            
        // assert
        html.HtmlShouldBe("<th><span>Name</span></th>");
    }
    
    [Test]
    public async Task If_header_has_content_then_should_keep_header_contents()
    {
        // arrange
        var model = new Result()
        {
            Products = new List<Product>()
            {
                new() { Name = "iPhone" },
                new() { Name = "Samsung" },
            }
        };
        var tag = CreateTagWithFor(new ThTagHelper(), model, x => x.Products[0].Name);

        // act
        var html = await ProcessTagAsync(tag, "miru-th", childContent: "Product Name");
            
        // assert
        html.HtmlShouldBe("<th>Product Name</th>");
    }
    
    [Test]
    public async Task If_there_is_no_model_then_should_create_empty_header()
    {
        // arrange
        var tag = CreateTag(new ThTagHelper());

        // act
        var html = await ProcessTagAsync(tag, "miru-th");
            
        // assert
        html.HtmlShouldBe("<th></th>");
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