using System;
using Miru.Domain;

namespace SelfImprov.Domain
{
    public class Achievement : Entity, ITimeStamped
    {
        public long IterationId { get; set; }
        public long GoalId { get; set; }
        
        public bool Achieved { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public Goal Goal { get; protected set; }
        public Iteration Iteration { get; set; }
    }
}