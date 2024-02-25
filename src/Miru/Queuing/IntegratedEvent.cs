using MediatR;

namespace Miru.Queuing;

public abstract class IntegratedEvent2 : INotification, IQueueable
{
    private readonly FeatureInfo _featureInfo;
    
    public override string ToString() => _featureInfo.GetTitle();

    protected IntegratedEvent2()
    {
        // TODO: no need to use FeatureInfo, set the Title directly
        _featureInfo = new FeatureInfo(this);
    }
}

public abstract record IntegratedEvent : INotification, IQueueable
{
    private readonly FeatureInfo _featureInfo;
    
    public sealed override string ToString() => _featureInfo.GetTitle();

    protected IntegratedEvent()
    {
        // TODO: no need to use FeatureInfo, set the Title directly
        _featureInfo = new FeatureInfo(this);
    }
}