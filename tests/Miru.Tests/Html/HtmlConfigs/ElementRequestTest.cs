using System.Collections.Generic;
using Miru.Html;
using Miru.Html.HtmlConfigs;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class ElementRequestTest : MiruTagTesting
{
    private ElementNaming _naming;

    [SetUp]
    public void Setup()
    {
        _naming = _.Get<ElementNaming>();
    }
    
    [Test]
    public void Should_build_from_x_for()
    {
        // arrange
        var model = new InterestField { Name = "Music" };
        var tag = TagWithXFor(new InputTagHelper(), model, x => x.Name);
        
        // act
        var elementRequest = ElementRequest.Create(tag);
        
        // assert
        elementRequest.Name.ShouldBe("Name");
        elementRequest.PropertyName.ShouldBe("Name");
        elementRequest.Value.ShouldBe("Music");
        elementRequest.Naming.ShouldBe(_naming);
        elementRequest.Model.ShouldBe(model);
    }
    
    [Test]
    public void Should_build_from_x_for_with_children_property()
    {
        // arrange
        var model = new Command
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
        var tag = TagWithXFor(new InputTagHelper(), model, x => x.Interests[2].InterestFields[1].Name);
        
        // act
        var elementRequest = ElementRequest.Create(tag);
        
        // assert
        elementRequest.Name.ShouldBe("Interests[2].InterestFields[1].Name");
        elementRequest.PropertyName.ShouldBe("Name");
        elementRequest.Value.ShouldBe("Genre");
        elementRequest.Naming.ShouldBe(_naming);
        elementRequest.Model.ShouldBe(model);
    }
    
    [Test]
    public void Should_build_from_for()
    {
        // arrange
        var model = new InterestField { Id = "sports" };
        var tag = TagWithFor(new InputTagHelper(), model, x => x.Id);
        
        // act
        var elementRequest = ElementRequest.Create(tag);
        
        // assert
        elementRequest.Name.ShouldBe("Id");
        elementRequest.Value.ShouldBe("sports");
        elementRequest.Naming.ShouldBe(_naming);
        elementRequest.Model.ShouldBe(model);
    }

    [Test]
    public void Should_build_from_for_with_children_property()
    {
        var model = new Command
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
        var tag = TagWithFor(new InputTagHelper(), model, x => x.Interests[2].InterestFields[1].Name);
        
        // act
        var elementRequest = ElementRequest.Create(tag);
        
        // assert
        elementRequest.Name.ShouldBe("Interests[2].InterestFields[1].Name");
        elementRequest.PropertyName.ShouldBe("Name");
        elementRequest.Value.ShouldBe("Genre");
        elementRequest.Naming.ShouldBe(_naming);
        elementRequest.Model.ShouldBe(model);
    }

    [Test]
    public void Should_build_from_model_expression_with_children_property()
    {
    }

    [Test]
    public void Should_build_from_model_expression()
    {
    }
    
    [Test]
    public void Should_be_assignable_for_reference_types()
    {
        // arrange
        var model = new Interest();
        var tag = TagWithFor(new InputTagHelper(), model, x => x.Category);
        
        // act
        var elementRequest = ElementRequest.Create(tag);
        
        // assert
        elementRequest.IsAssignable<Category>().ShouldBeTrue();
        elementRequest.IsAssignable<ICategory>().ShouldBeTrue();
    }
    
    public class Command
    {
        public List<Interest> Interests { get; set; } = new();
    }
        
    public class Interest
    {
        public List<InterestField> InterestFields { get; set; } = new();
        public Category Category { get; set; }
    }
        
    public class InterestField
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    
    public class Category : ICategory
    {
        public string Name { get; set; }
    }

    public interface ICategory
    {
        string Name { get; }
    }
}