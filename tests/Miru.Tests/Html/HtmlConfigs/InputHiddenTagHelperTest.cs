using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputHiddenTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_hidden_input_and_property_id()
    {
        // arrange
        var model = new Command { PersonId = 12 };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.PersonId);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input name=""PersonId"" id=""PersonId"" type=""hidden"" value=""12"" />");
    }
    
    [Test]
    public void If_has_attr_type_hidden_then_should_render_hidden()
    {
        // arrange
        var model = new Command { PersonId = 12 };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.PersonId);

        // act
        var output = ProcessTag(tag, "miru-input2", new { type = "hidden" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""hidden"" name=""PersonId"" id=""PersonId"" value=""12"" />");
    }
    
    [Test]
    public void Input_hidden_x_for_and_flat_property()
    {
        // arrange
        var model = new Command { Name = "iPhone" };
        var tag = TagWithXFor(new InputTagHelper(), model, x => x.Name);

        // act
        var output = ProcessTag(tag, "miru-input2", new { type = "hidden" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""hidden"" name=""Name"" id=""Name"" value=""iPhone"" />");
    }
    
    [Test]
    public void Input_hidden_for_smart_enum()
    {
        // arrange
        var model = new Command { Status = Statuses.Inactive };
        var tag = TagWithFor(new InputTagHelper(), model, x => x.Status);

        // act
        var output = ProcessTag(tag, "miru-input2", new { type = "hidden" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @$"<input type=""hidden"" name=""Status"" id=""Status"" value=""{Statuses.Inactive.Value}"" />");
    }
    
    // [Test]
    // public void Input_hidden_x_for_attr_value_and_boolean_property()
    // {
    //     // arrange
    //     var model = new Command { SelectedValue = "iPhone" };
    //     var tag = TagWithXFor(new InputTagHelper2(), model, x => x.SelectedValue);
    //
    //     // act
    //     var output = ProcessTag(tag, "miru-input2", new { type = "hidden" });
    //         
    //     // assert
    //     output.HtmlShouldBe(@"<input type=""hidden"" name=""Name"" id=""Name"" value=""iPhone"" />");
    // }
    
    public class Command
    {
        public long PersonId { get; set; }
        public string Name { get; set; }
        public string SelectedValue { get; set; }
        public Statuses Status { get; set; }
    }

    public class Statuses : Ardalis.SmartEnum.SmartEnum<Statuses>
    {
        public readonly static Statuses Active = new(1, "Ativo");
        public readonly static Statuses Inactive = new(2, "Inativo");
        
        public Statuses(int value, string name) : base(name, value)
        {
        }
    }
}