using System;
using Miru;
using Miru.Domain;
using Miru.Userfy;
using Mong.Features.Password;

namespace Mong.Domain
{
    public class User : Entity, IUser, IRecoverable, IReceivesEmail, IConfirmable, ITimeStamped, ICanBeAdmin, IBlockable
    {
        public string Display => Email;

        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Name { get; set; }
        
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordSentAt { get; set; }
        
        public string ConfirmationToken { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? ConfirmationSentAt { get; set; }
        
        public DateTime? BlockedAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public bool IsAdmin { set; get; }

        public void ChangePassword(PasswordEdit.Command request)
        {
            if (HashedPassword.NotEqual(Hash.Create(request.CurrentPassword)))
            {
                throw new DomainException("Current password does not match");
            }
            
            HashedPassword = Hash.Create(request.Password);
        }
        
        public void BlockOrUnblock()
        {
            if (IsBlocked())
                BlockedAt = null;
            else
                BlockedAt = DateTime.Now;
        }

        public bool IsBlocked() => BlockedAt.HasValue;
    }
}