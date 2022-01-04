using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Miru.Wizardable;

public abstract class WizardForm
{
    private readonly List<WizardStepInfo> _steps = new();

    [JsonInclude] 
    public int CurrentIndex { get; private set; } = 0;

    [JsonIgnore]
    public List<WizardStepInfo> Steps => _steps;

    [JsonIgnore]
    public WizardStepInfo CurrentStep => _steps[CurrentIndex];
    
    public void JumpTo(IWizardRequest step)
    {
        var index = IndexFor(step.GetType());
        
        CurrentIndex = index;
    }
    
    public bool IsCurrentStep(IWizardRequest request) => 
        IndexFor(request.GetType()) == CurrentIndex;

    public bool IsCurrentStep(WizardStepInfo step) => 
        IndexFor(step.Type) == CurrentIndex;

    public bool IsFutureStep(IWizardRequest step) => 
        IndexFor(step.GetType()) > CurrentIndex;

    public bool IsFutureStep(WizardStepInfo step) => 
        IndexFor(step.Type) > CurrentIndex;

    public bool IsPastStep(IWizardRequest step) => 
        IndexFor(step.GetType()) < CurrentIndex;

    public bool IsPastStep(WizardStepInfo step) => 
        IndexFor(step.Type) < CurrentIndex;

    public bool IsFilled(WizardStepInfo step) =>
        step.GetCommand().Filled;

    protected void AddStep<TStep>(string stepName, Func<TStep> step) 
        where TStep : IWizardRequest
    {
        _steps.Add(new WizardStepInfo
        {
            Name = stepName,
            GetCommand = () => step(),
            Type = typeof(TStep)
        });
    }

    public TResponse CommandFor<TResponse>(WizardRequest<TResponse> request)
    {
        var index = IndexFor(typeof(TResponse));

        var step = _steps[index];
        
        var command = step.GetCommand();

        command.Review = request.Review;
        command.Filled = request.Filled;
        
        return (TResponse) step.GetCommand();
    }

    public FeatureResult JumpFor(IWizardRequest request)
    {
        CurrentStep.GetCommand().Filled = true;
        
        if (request.Review)
        {
            CurrentIndex = Steps.Count - 1;
        }
        else
        {
            var index = IndexFor(request.GetType());

            CurrentIndex = index + 1;
        }
        
        var step = _steps[CurrentIndex];

        return new FeatureResult(Activator.CreateInstance(GetFeature(step.Type)));
    }

    private Type GetFeature(Type type) => 
        type.ReflectedType ?? type;

    private int IndexFor(Type type) => 
        _steps.FindIndex(x => x.Type.Implements(type));
}