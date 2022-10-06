using MediatR;

namespace Miru.Queuing;

public abstract class MiruNotification : INotification, IQueueable
{
    private readonly FeatureInfo _featureInfo;
    
    public override string ToString() => _featureInfo.GetTitle();

    protected MiruNotification()
    {
        _featureInfo = new FeatureInfo(this);
    }
}