namespace Miru.Queuing;

public interface IQueueable
{
    string Id { get; }
}

public static class QueueableExtensions
{
    public static string Title(this IQueueable queueable)
    {
        var type = queueable.GetType();

        if (queueable.Id != default)
        {
            var reflectedType = type.ReflectedType;
        
            if (reflectedType is not null)
                return $"{reflectedType.Name}?Id={queueable.Id}";
                
            return $"{type.Name}?Id={queueable.Id}";
        }

        return type.Name;
    }
}
