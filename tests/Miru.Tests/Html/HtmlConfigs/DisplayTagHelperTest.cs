using System.ComponentModel.DataAnnotations;
using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class DisplayTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_string_property()
    {
        // arrange
        var model = new Result { Name = "iPhone" };
        var tag = TagWithFor(new DisplayTagHelper(), model, m => m.Name);

        // act
        var html = ProcessTag(tag, "miru-display2");

        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(html, "<span>iPhone</span>");
    }

    [Test]
    public void Should_render_int_property()
    {
        // arrange
        var model = new Result { Age = 44 };
        var tag = TagWithFor(new DisplayTagHelper(), model, m => m.Age);

        // act
        var html = ProcessTag(tag, "miru-display2");

        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(html, "<span>44</span>");
    }
    
    [Test]
    public void Should_render_enum_value()
    {
        // arrange
        var model = new Result { Status = Status.Inactive };
        var tag = TagWithFor(new DisplayTagHelper(), model, m => m.Status);

        // act
        var html = ProcessTag(tag, "miru-display2");

        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(html, "<span>Inactive</span>");
    }
    
    [Test]
    public void Should_render_enum_display_attribute_value()
    {
        // arrange
        var model = new Result { Status = Status.Active };
        var tag = TagWithFor(new DisplayTagHelper(), model, m => m.Status);

        // act
        var html = ProcessTag(tag, "miru-display2");

        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(html, "<span>Akitv</span>");
    }
    
    public class Result
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        [Display(Name = "Akitv")]
        Active,
        Inactive
    }
}