using HtmlTags;
using Miru.Html;

namespace Miru.Tests.Html;

[TestFixture]
public class HtmlTagExtensionsTest
{
    public class AttrAppend
    {
        [Test]
        public void Should_add_attr_if_attribute_does_not_exist()
        {
            // arrange
            var form = new HtmlTag("form");
            
            // act
            form.AttrAppend("data-controller", "validation");
            
            // assert
            form.ToString().ShouldBe("<form data-controller=\"validation\"></form>");
        }
        
        [Test]
        public void Should_append_value_to_attr_attribute_if_already_has_values()
        {
            // arrange
            var form = new HtmlTag("form").Attr("data-controller", "validation");
            
            // act
            form.AttrAppend("data-controller", "autofocus");
            
            // assert
            form.ToString().ShouldBe("<form data-controller=\"validation autofocus\"></form>");
        }
    }
}