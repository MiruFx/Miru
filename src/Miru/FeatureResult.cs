using System;
using System.Collections.Generic;
using MediatR;
using Vereyon.Web;

namespace Miru;

public class FeatureResult
{
    public Dictionary<FlashMessageType, string> Messages { get; } = new();

    public FeatureResult(object model)
    {
        Model = model;
    }

    public object Model { get; set; }

    public Type Type => Model.GetType();
    
    public void AddMessage(FlashMessageType type, string message)
    {
        Messages.Add(type, message);
    }
}

public class FeatureResult<TFeature> : FeatureResult where TFeature : new()
{
    public FeatureResult() : base(new TFeature())
    {
    }

    public FeatureResult(TFeature instance) : base(instance)
    {
    }
}

public static class FeatureResultExtensions
{
    public static FeatureResult Success(this FeatureResult result, string message)
    {
        result.AddMessage(FlashMessageType.Confirmation, message);
        return result;
    }
        
    public static FeatureResult Alert(this FeatureResult result, string message)
    {
        result.AddMessage(FlashMessageType.Warning, message);
        return result;
    }
        
    public static FeatureResult Danger(this FeatureResult result, string message)
    {
        result.AddMessage(FlashMessageType.Danger, message);
        return result;
    }
        
    public static FeatureResult Info(this FeatureResult result, string message)
    {
        result.AddMessage(FlashMessageType.Info, message);
        return result;
    }
}