using System;

namespace Miru.Behaviors.TimeStamp
{
    public interface ITimeStamped
    {
        DateTime CreatedAt { get; set; }
        
        DateTime UpdatedAt { get; set; }
    }
}