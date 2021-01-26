using System;

namespace Miru.Domain
{
    public interface ITimeStamped
    {
        DateTime CreatedAt { get; set; }
        
        DateTime UpdatedAt { get; set; }
    }
}