using Miru.Testing;
using Miru.Wizardable;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Wizardable;

public class WizardFormTest
{
    [Test]
    public void Should_return_step_for_first_query()
    {
        // arrange
        var form = new SignUpForm();
        var query = new ConfirmPassword.Query();

        // act
        var command = form.CommandFor(query);
        
        // assert
        command.ShouldBeOfType<ConfirmPassword.Command>();
        
        form.CurrentStep.GetCommand().ShouldBe(command);
        form.CurrentStep.Name.ShouldBe("Confirm Password");
    }

    [Test]
    public void Should_return_next_step_for_first_command()
    {
        // arrange
        var form = new SignUpForm();
        var command = new ConfirmPassword.Command();

        // act
        var result = form.JumpFor(command);
        
        // assert
        result.ShouldBeOfType<FeatureResult>();
        
        form.CurrentStep.GetCommand().ShouldBeOfType<ConfirmProfile.Command>(); // must have moved
        form.CurrentStep.Name.ShouldBe("Confirm Profile");
    }
    
    [Test]
    public void Should_list_all_steps()
    {
        // arrange
        var form = new SignUpForm();

        // act
        var steps = form.Steps;
        
        // assert
        steps.ShouldCount(3);
        steps.At(0).GetCommand().ShouldBeOfType<ConfirmPassword.Command>();
        steps.At(1).GetCommand().ShouldBeOfType<ConfirmProfile.Command>();
        steps.At(2).GetCommand().ShouldBeOfType<FindFriends.Command>();
    }
    
    [Test]
    public void Should_return_if_model_is_current_step()
    {
        // arrange
        var form = new SignUpForm();

        // act
        form.JumpTo(new ConfirmProfile.Command());
        
        // assert
        form.IsCurrentStep(new ConfirmPassword.Command()).ShouldBeFalse();
        form.IsCurrentStep(new ConfirmProfile.Command()).ShouldBeTrue();
        form.IsCurrentStep(new FindFriends.Command()).ShouldBeFalse();
    }
    
    [Test]
    public void Should_return_if_model_is_future_step()
    {
        // arrange
        var form = new SignUpForm();

        // act
        form.JumpTo(new ConfirmProfile.Command());
        
        // assert
        form.IsFutureStep(new ConfirmPassword.Command()).ShouldBeFalse();
        form.IsFutureStep(new ConfirmProfile.Command()).ShouldBeFalse();
        form.IsFutureStep(new FindFriends.Command()).ShouldBeTrue();
    }
    
    [Test]
    public void Should_return_if_model_is_past_step()
    {
        // arrange
        var form = new SignUpForm();

        // act
        form.JumpTo(new ConfirmProfile.Command());
        
        // assert
        form.IsPastStep(new ConfirmPassword.Command()).ShouldBeTrue();
        form.IsPastStep(new ConfirmProfile.Command()).ShouldBeFalse();
        form.IsPastStep(new FindFriends.Command()).ShouldBeFalse();
    }
    
    [Test]
    public void If_reviewing_command_for_step_should_have_review_true()
    {
        // arrange
        var form = new SignUpForm();
        var query = new ConfirmPassword.Query { Review = true };

        // act
        var command = form.CommandFor(query);
        
        // assert
        command.Review.ShouldBeTrue();
        
        form.CurrentStep.GetCommand().ShouldBeOfType<ConfirmPassword.Command>();
    }
    
    [Test]
    public void If_reviewing_next_step_should_be_last_step()
    {
        // arrange
        var form = new SignUpForm();
        var command = new ConfirmPassword.Command { Review = true };

        // act
        var result = form.JumpFor(command);
        
        // assert
        result.ShouldBeOfType<FeatureResult>();
        
        form.CurrentStep.GetCommand().ShouldBeOfType<FindFriends.Command>();
    }
    
    [Test]
    public void Should_set_updated_filled_status_when_step_advances()
    {
        // arrange
        var form = new SignUpForm();
        var command = new ConfirmProfile.Command { Review = true };
        form.JumpFor(new ConfirmPassword.Command());

        // act
        form.JumpFor(command);
        
        // assert
        form.ConfirmPassword.Filled.ShouldBeTrue();
        form.ConfirmProfile.Filled.ShouldBeTrue();
        form.FindFriends.Filled.ShouldBeFalse();
    }
    
    public class SignUpForm : WizardForm
    {
        public SignUpForm()
        {
            AddStep("Confirm Password", () => ConfirmPassword);
            AddStep("Confirm Profile", () => ConfirmProfile);
            AddStep("Find Friends", () => FindFriends);
        }
        
        public ConfirmPassword.Command ConfirmPassword { get; set; } = new();
        public ConfirmProfile.Command ConfirmProfile { get; set; } = new();
        public FindFriends.Command FindFriends { get; set; } = new();
    }

    public class ConfirmPassword
    {
        public class Query : WizardRequest<Command>
        {
        }

        public class Command : WizardRequest<FeatureResult>
        {
        }
    }
    
    public class ConfirmProfile
    {
        public class Query : WizardRequest<Command>
        {
        }

        public class Command : WizardRequest<FeatureResult>
        {
        }
    }
    
    public class FindFriends
    {
        public class Query : WizardRequest<Command>
        {
        }

        public class Command : WizardRequest<FeatureResult>
        {
        }
    }
}