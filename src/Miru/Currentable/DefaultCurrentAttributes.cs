namespace Miru.Currentable;

public class DefaultCurrentAttributes : ICurrentHandler
{
    public Task Handle<TRequest>(TRequest request, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}