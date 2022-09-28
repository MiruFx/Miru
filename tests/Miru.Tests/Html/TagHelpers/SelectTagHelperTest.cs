using System;
using System.Collections.Generic;
using AV.Enumeration;
using Miru.Domain;
using Miru.Html.Tags;
using Miru.Mvc;
using NUnit.Framework;
using Playground.Features.Examples;

namespace Miru.Tests.Html.TagHelpers;

public class SelectTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task Can_create_select_for_a_lookup_from_dictionary()
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
        var tag = CreateTagWithFor(new SelectTagHelper(), model, m => m.Address.Country);
        tag.Lookup = MakeExpression(model.Countries);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-select");

        // assert
        html.HtmlShouldBe(
            "<select name=\"Address.Country\" id=\"Address_Country\"><option value=\"br\">Brazil</option><option value=\"de\" selected=\"selected\">Germany</option></select>");
    }
    
    [Test]
    public async Task Should_create_select_for_a_lookup_from_special_enumeration()
    {
        // arrange
        var model = new Command { Status = Statuses.Finished };
        var tag = CreateTagWithFor(new SelectTagHelper(), model, m => m.Status);
        tag.Lookup = MakeExpression(model.StatusLookups);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-select");
    
        // assert
        html.HtmlShouldBe(
            "<select name=\"Status\" id=\"Status\"><option value=\"1\">Pending</option><option value=\"2\" selected=\"selected\">Finished</option></select>");
    }
    
    [Test]
    public async Task Should_create_select_for_enum_lookup()
    {
        // arrange
        var model = new Command { City = Cities.Berlin };
        var tag = CreateTagWithFor(new SelectTagHelper(), model, m => m.City);
        tag.Lookup = MakeExpression(model.CitiesLookup);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-select");
    
        // assert
        html.HtmlShouldBe(
            @"<select name=""City"" id=""City""><option value=""0"">Krakow</option><option value=""1"">Dublin</option><option value=""2"" selected=""selected"">Berlin</option></select>");
    }

    public class Command
    {
        public Statuses Status { get; set; }
        public Cities City { get; set; }
        
        // lookups
        public SelectLookups StatusLookups => Enumeration.GetAll<Statuses>().ToSelectLookups();
        public SelectLookups CitiesLookup => Enum.GetValues<Cities>().ToSelectLookups();
    }

    public enum Cities
    {
        Krakow,
        Dublin,
        Berlin
    }
    
    public class Statuses : Enumeration
    {
        public static readonly Statuses Pending = new(1, nameof(Pending));
        public static readonly Statuses Finished = new(2, nameof(Finished));
        
        public Statuses(int value, string name) : base(value, name)
        {
        }
    }
}