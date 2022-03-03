using Miru.Queuing;

namespace Corpo.Skeleton.Features.Teams;

public class TeamCreated
{
    public class Job : IMiruJob
    {
    }

    public class Handler : IRequestHandler<Job>
    {
        public Task<Unit> Handle(Job request, CancellationToken ct)
        {
            return Unit.Task;
        }
    }
}