using Miru.Queuing;

namespace Corpo.Skeleton.Features.Teams;

public class TeamCreated
{
    public class Job : MiruJob<Job>
    {
    }

    public class Handler : JobHandler<Job>
    {
        public override Task Handle(Job request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}