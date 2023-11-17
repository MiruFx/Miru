namespace Miru.Domain;

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message)
    {
    }
}