using System.Collections.Generic;
using Miru.Html.Tags;
using Miru.Mvc;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class SelectTagHelperTest : MiruTagTesting
{
    [Test]
    public void Select_for_a_lookup()
    {
        // arrange
        var model = new Command
        {
            Country = "de",
            Countries = new Dictionary<string, string>()
            {
                {"br", "Brazil"},
                {"de", "Germany"}
            }.ToSelectLookups(), 
        };
        
        var tag = TagWithFor(
            new SelectTagHelper { Lookups = model.Countries }, 
            model, 
            m => m.Country);
            
        // act
        var html = ProcessTag(tag, "miru-select2");

        // assert
        Helpers.Extensions.HtmlShouldBe(html, "<select name=\"Country\" id=\"Country\"><option value=\"br\">Brazil</option><option value=\"de\" selected=\"selected\">Germany</option></select>");
    }
    
    [Test]
    public void Select_with_empty_option_for_a_lookup()
    {
        // arrange
        var model = new Command
        {
            Country = "de",
            Countries = new Dictionary<string, string>()
            {
                {"br", "Brazil"},
                {"de", "Germany"}
            }.ToSelectLookups(), 
        };
        
        var tag = TagWithFor(
            new SelectTagHelper { Lookups = model.Countries }, 
            model, 
            m => m.Country);
            
        // act
        var html = ProcessTag(tag, "miru-select2", new { empty_option = string.Empty});

        // assert
        Helpers.Extensions.HtmlShouldBe(html, "<select name=\"Country\" id=\"Country\"><option></option><option value=\"br\">Brazil</option><option value=\"de\" selected=\"selected\">Germany</option></select>");
    }
   
    [Test]
    public void Select_for_a_system_enum_property()
    {
        // arrange
        var model = new Command { Status = Statuses.Finished };
        
        var tag = TagWithFor(
            new SelectTagHelper { Lookups = Statuses.List.ToSelectLookups() }, 
            model, 
            m => m.Status);
        
        // act
        var html = ProcessTag(tag, "miru-select2");
    
        // assert
        Helpers.Extensions.HtmlShouldBe(html, "<select name=\"Status\" id=\"Status\"><option value=\"1\">Pending</option><option value=\"2\" selected=\"selected\">Finished</option></select>");
    }
    
    public class Command
    {
        public Statuses Status { get; set; }
        public string Country { get; set; }    
        public Cities City { get; set; }
        
        // lookups
        public SelectLookups Countries { get; set; }
    }
    
    public enum Cities
    {
        Krakow = 1,
        Dublin = 2,
        Berlin = 3
    }
    
    public class Statuses : Ardalis.SmartEnum.SmartEnum<Statuses>
    {
        public static readonly Statuses Pending = new(1, nameof(Pending));
        public static readonly Statuses Finished = new(2, nameof(Finished));
        
        public Statuses(int value, string name) : base(name, value)
        {
        }
    }
}