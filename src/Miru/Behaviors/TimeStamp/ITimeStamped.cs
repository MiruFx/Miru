using System;

namespace Miru.Behaviors.TimeStamp;

public interface ITimeStamped
{
    DateTime CreatedAt { get; set; }
        
    DateTime UpdatedAt { get; set; }
}

public static class TimeStampedExtensions
{
    public static void TimeStamp(this ITimeStamped timeStamped)
    {
        timeStamped.CreatedAt = DateTime.Now;
        timeStamped.UpdatedAt = DateTime.Now;
    }
}