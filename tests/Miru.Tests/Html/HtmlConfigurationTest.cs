using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Miru.Html;

namespace Miru.Tests.Html;

[TestFixture]
public class HtmlConfigurationTest
{
    private ITestFixture _;
    private readonly HtmlGenerator _html;

    public HtmlConfigurationTest()
    {
        _ = new ServiceCollection()
            .AddMiruHtml(new HtmlConfiguration())
            .AddMiruTestFixture()
            .BuildServiceProvider()
            .GetRequiredService<ITestFixture>();

        _html = _.Get<HtmlGenerator>();
    }
    
    [Test]
    public void Should_configure_form_file_input()
    {
        // arrange
        var profileEdit = new ProfileEdit();
        
        // act
        var tag = _html.InputFor(profileEdit, x => x.Picture);

        // assert
        tag.Attr("type").ShouldBe("file");
    }
    
    [Test]
    public void Should_configure_list_of_form_file_inputs()
    {
        // arrange
        var profileEdit = new ProfileEdit();
        
        // act
        var tag = _html.InputFor(profileEdit, x => x.Photos);

        // assert
        tag.Attr("type").ShouldBe("file");
        tag.Attr("multiple").ShouldBe("multiple");
    }

    public class HtmlConfig : HtmlConfiguration
    {
    }
    
    public class ProfileEdit
    {
        public IFormFile Picture { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}