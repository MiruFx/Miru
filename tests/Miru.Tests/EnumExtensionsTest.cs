using System.ComponentModel.DataAnnotations;

namespace Miru.Tests;

public class EnumExtensionsTest
{
    public class DisplayName
    {
        [Test]
        public void Should_return_display_name()
        {
            var city = Cities.Krakow;
            city.DisplayName().ShouldBe("Cracovia");
        }
        
        [Test]
        public void If_enum_doesnt_have_the_value_then_should_return_empty()
        {
            Cities city = 0;
            city.DisplayName().ShouldBeEmpty();
        }
    }

    public enum Cities
    {
        [Display(Name = "Cracovia")]
        Krakow = 1,
        Dublin = 2
    }
}