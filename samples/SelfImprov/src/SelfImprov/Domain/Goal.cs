using System;
using Miru.Domain;

namespace SelfImprov.Domain
{
    public class Goal : Entity, ITimeStamped, IInactivable, IBelongsToUser
    {
        public string Name { get; set; }
        
        public long AreaId { get; set; }
        public long UserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public bool IsInactive { get; set; }

        public Area Area { get; set; }        
        public User User { get; set; }
    }
}