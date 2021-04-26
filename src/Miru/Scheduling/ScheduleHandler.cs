using MediatR;

namespace Miru.Scheduling
{
    public abstract class ScheduleHandler<TTask> : RequestHandler<TTask> where TTask : IScheduledTask
    {
    }
}