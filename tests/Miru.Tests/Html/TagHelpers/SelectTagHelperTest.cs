using System.Collections.Generic;
using Corpo.Skeleton.Features.Examples;
using Miru.Html.Tags;
using Miru.Mvc;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class SelectTagHelperTest : TagHelperTest
    {
        [Test]
        public void Can_create_select_for_a_lookup_from_dictionary()
        {
            // arrange
            var countries = new Dictionary<string, string>()
            {
                {"br", "Brazil"},
                {"de", "Germany"}
            };
            var model = new ExampleForm.Command
            {
                Countries = countries.ToLookups(), 
                Address = {Country = "de"}
            };
            var tag = CreateTag(new SelectTagHelper(), model, m => m.Address.Country);
            tag.Lookup = MakeExpression(model.Countries);
            
            // act
            var html = ProcessTag(tag, "miru-select");

            // assert
            html.PreElement.GetContent().ShouldBe(
                "<select name=\"Address.Country\" id=\"Address_Country\"><option value=\"br\">Brazil</option><option value=\"de\" selected=\"selected\">Germany</option></select>");
        }
    }
}