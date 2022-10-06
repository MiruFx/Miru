using MediatR;
using Miru.Pipeline;

namespace Miru.Queuing;

public interface IMiruJob : IInvokable, IQueueable
{
}

public abstract class MiruJob<TJob> : IRequest<TJob>, IMiruJob
{
    private readonly FeatureInfo _featureInfo;
    
    public override string ToString() => _featureInfo.GetTitle();

    protected MiruJob()
    {
        _featureInfo = new FeatureInfo(this);
    }
}
