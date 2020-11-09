using System;

namespace Mong.Domain
{
    public interface IBlockable
    {
        DateTime? BlockedAt { get; set; }
    }
}