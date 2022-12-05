using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputPasswordTagHelperTest : MiruTagTesting
{
    [Test]
    public void Input_for_password_property()
    {
        // arrange
        var model = new Command { Password = "123" };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Password);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input name=""Password"" id=""Password"" type=""password"" value=""123"" />");
    }
    
    [Test]
    public void Input_for_property_name_ending_with_password()
    {
        // arrange
        var model = new Command { ConfirmPassword = "456" };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.ConfirmPassword);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input name=""ConfirmPassword"" id=""ConfirmPassword"" type=""password"" value=""456"" />");
    }
    
    public class Command
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}