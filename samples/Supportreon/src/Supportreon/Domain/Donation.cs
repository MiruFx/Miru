using System;
using Miru.Behaviors.BelongsToUser;
using Miru.Domain;
using Supportreon.Features.Donations;

namespace Supportreon.Domain
{
    public class Donation : Entity, IBelongsToUser<User>, ITimeStamped
    {
        public static decimal Minimum = 5;

        public long ProjectId { get; set; }
        public decimal Amount { get; set; }
        public string CreditCard { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public Project Project { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsRecurrent { get; set; }

        public Donation()
        {
        }

        public Donation(DonationNew.Command request, Project project, User donor)
        {
            // domain validations
            if (project.MinimumDonation > request.Amount)
                throw new DomainException($"The minimum donation for this project is {project.MinimumDonation}");

            if (project.EndDate.HasValue && DateTime.Today > project.EndDate)
                throw new DomainException(
                    $"Is not possible to donate to a ended project. This project ended at {project.EndDate}");

            Amount = request.Amount;
            CreditCard = request.CreditCard;
            IsRecurrent = request.IsRecurrent;
            ProjectId = project.Id;
            User = donor;
            UserId = donor.Id;

            project.TotalDonations++;
            project.TotalAmount += request.Amount;
        }
    }
}
