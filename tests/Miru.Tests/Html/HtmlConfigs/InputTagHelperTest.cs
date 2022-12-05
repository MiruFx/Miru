using System.Collections.Generic;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputTagHelperTest : MiruTagTesting
{
    [Test]
    public void Input_attr_name_should_generate_id_from_name()
    {
        // arrange
        var command = new Command
        {
            Interests = new List<Interest>()
            {
                new(),
                new(),
                new()
                {
                    InterestFields = new List<InterestField>
                    {
                        new() { Name = "Category" },
                        new() { Name = "Genre" },
                    }
                }
            }
        };
        var interestField = command.Interests[2].InterestFields[1];
        var tag = TagWithFor(new InputTagHelper(), interestField, m => interestField.Name);

        // act
        var output = ProcessTag(tag, attributes: new { name = "Interests[2].InterestFields[1].Name" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""Interests[2].InterestFields[1].Name"" id=""Interests_2__InterestFields_1__Name"" type=""text"" value=""Genre"" />");
    }
    
    public class Command
    {
        public List<Interest> Interests { get; set; } = new();
    }
        
    public class Interest
    {
        public List<InterestField> InterestFields { get; set; } = new();
    }
        
    public class InterestField
    {
        public string Name { get; set; }
    }
}