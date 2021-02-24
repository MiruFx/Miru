using System;
using Miru.Behaviors.BelongsToUser;
using Miru.Domain;

namespace Supportreon.Domain
{
    public class Project : Entity, ITimeStamped, IBelongsToUser<User>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinimumDonation { get; set; }
        public DateTime CreatedAt { get ; set; }
        public DateTime UpdatedAt { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long CategoryId { get; set; }
        public decimal? Goal { get; set; }
        public DateTime? EndDate { get; set; }
        public long TotalDonations { get; set; }
        public decimal TotalAmount { get; set; }

        public bool IsActive => EndDate.HasValue == false || EndDate > DateTime.Now;
        public Category Category { get; set; }

        public void EndProject()
        {
            EndDate = DateTime.Now;
        }
    }
}
