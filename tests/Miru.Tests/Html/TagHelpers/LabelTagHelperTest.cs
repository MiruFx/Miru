using System.Collections.Generic;
using System.Threading.Tasks;
using Corpo.Skeleton.Features.Products;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class LabelTagHelperTest : TagHelperTest
    {
        private Command _request;

        [SetUp]
        public void Setup()
        {
            _request = new Command();
        }
        
        // [Test]
        // public async Task Should_render_label()
        // {
        //     // arrange
        //     var tag = new LabelTagHelper
        //     {
        //         For = MakeExpression(_request, m => m.Name),
        //         RequestServices = ServiceProvider
        //     };
        //
        //     // act
        //     var output = await ProcessTagAsync(tag, "miru-label");
        //     
        //     // assert
        //     output.TagName.ShouldBeNull();
        //     output.PreElement.GetContent().ShouldBe("<label for=\"Name\">Name</label>");
        // }
     
        public class Command
        {
            public string Name { get; set; }
        }
    }
}