using System.Collections.Generic;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class InputHiddenTagHelperTest : TagHelperTest
{
    [Test]
    public void Should_render_input_hidden_for_model()
    {
        // arrange
        var model = new Command
        {
            Products = new()
            {
                new Product() { ProductName = "iPhone" }
            }
        };
        var tag = CreateTag(new InputHiddenTagHelper(), model, m => m.Products[0].ProductName);
            
        // act
        var html = ProcessTag(tag, "miru-hidden");
            
        // assert
        html.PreElement.GetContent().ShouldBe(
            "<input type=\"hidden\" value=\"iPhone\" name=\"Products[0].ProductName\" id=\"Products_0__ProductName\">");
    }
       
    public class Command
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public string ProductName { get; set; }
    }
}