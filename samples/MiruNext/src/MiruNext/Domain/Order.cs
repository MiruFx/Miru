using System;
using Miru.Behaviors.TimeStamp;
using Miru.Domain;

namespace MiruNext.Domain;

public class Order : Entity, ITimeStamped
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}