using Vereyon.Web;

namespace Miru.Tests;

public class FeatureResultTest
{
    [Test]
    public void Should_create_feature_result_for_a_model()
    {
        // arrange
        var model = new UserList();

        // act
        var result = new FeatureResult(model);

        // arrange
        result.Model.ShouldBe(model);
    }
        
    [Test]
    public void Should_set_success_message_in_the_result()
    {
        // arrange
        var model = new UserList();

        // act
        var result = new FeatureResult(model).Success("Redirect successfully");

        // arrange
        result.Messages.At(0).Key.ShouldBe(FlashMessageType.Confirmation);
        result.Messages.At(0).Value.ShouldBe("Redirect successfully");
    }

    public class UserList
    {
    }
}