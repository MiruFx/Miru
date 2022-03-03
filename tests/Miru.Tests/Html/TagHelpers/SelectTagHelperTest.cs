using System.Collections.Generic;
using Miru.Domain;
using Miru.Html.Tags;
using Miru.Mvc;
using NUnit.Framework;
using Playground.Features.Examples;

namespace Miru.Tests.Html.TagHelpers;

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
            Countries = countries.ToSelectLookups(), 
            Address = {Country = "de"}
        };
        var tag = CreateTag(new SelectTagHelper(), model, m => m.Address.Country);
        tag.Lookup = MakeExpression(model.Countries);
            
        // act
        var html = ProcessTag(tag, "miru-select");

        // assert
        html.HtmlShouldBe(
            "<select name=\"Address.Country\" id=\"Address_Country\"><option value=\"br\">Brazil</option><option value=\"de\" selected=\"selected\">Germany</option></select>");
    }
    
    [Test]
    public void Should_create_select_for_a_lookup_from_special_enumeration()
    {
        // arrange
        var model = new Command { Status = Statuses.Finished };
        var tag = CreateTag(new SelectTagHelper(), model, m => m.Status);
        tag.Lookup = MakeExpression(model.StatusLookups);
            
        // act
        var html = ProcessTag(tag, "miru-select");
    
        // assert
        html.HtmlShouldBe(
            "<select name=\"Status\" id=\"Status\"><option value=\"1\">Pending</option><option value=\"2\" selected=\"selected\">Finished</option></select>");
    }

    public class Command
    {
        public Statuses Status { get; set; }
        
        // lookups
        public SelectLookups StatusLookups => Statuses.GetAll().ToSelectLookups();
    }

    public class Statuses : Enumeration<Statuses>
    {
        public static readonly Statuses Pending = new(1, nameof(Pending));
        public static readonly Statuses Finished = new(2, nameof(Finished));
        
        public Statuses(int value, string name) : base(value, name)
        {
        }
    }
}