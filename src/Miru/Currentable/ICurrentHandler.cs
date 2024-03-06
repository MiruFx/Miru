namespace Miru.Currentable;

public interface ICurrentHandler
{
    Task Handle<TRequest>(TRequest request, CancellationToken ct);
}