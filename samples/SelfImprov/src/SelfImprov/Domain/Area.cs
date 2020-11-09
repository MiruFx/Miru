using System;
using System.Collections.Generic;
using Miru.Domain;

namespace SelfImprov.Domain
{
    public class Area : Entity, ITimeStamped, IBelongsToUser, IInactivable
    {
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public long UserId { get; set; }

        public User User { get; protected set; }        
        public virtual ICollection<Goal> Goals { get; set; } = new HashSet<Goal>();
        
        public bool IsInactive { get; set; }
    }
}