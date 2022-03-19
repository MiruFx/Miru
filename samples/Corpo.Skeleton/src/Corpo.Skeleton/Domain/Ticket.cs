using Miru.Behaviors.TimeStamp;
using Miru.Domain;

namespace Corpo.Skeleton.Domain;

public class Ticket : Entity, ITimeStamped
{
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}