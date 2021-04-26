using MediatR;

namespace Miru.Queuing
{
    public abstract class JobHandler<TJob> : RequestHandler<TJob> where TJob : IMiruJob
    {
    }
}