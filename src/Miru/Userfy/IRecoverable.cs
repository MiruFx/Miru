using System;
using FluentValidation;

namespace Miru.Userfy
{
    public interface IRecoverable
    {
        string ResetPasswordToken { get; set; }
        
        DateTime? ResetPasswordSentAt { get; set; }
        
        string HashedPassword { get; set; }
    }

    public static class RecoverableExtensions
    {
        public static void RequestedPasswordReset(this IRecoverable account)
        {
            account.ResetPasswordToken = Guid.NewGuid().ToString();
            account.ResetPasswordSentAt = DateTime.Now;
        }
        
        public static void ResetPassword(this IRecoverable account, string newPassword)
        {
            account.ResetPasswordToken = string.Empty;
            account.ResetPasswordSentAt = null;
            account.HashedPassword = newPassword;
        }
        
        public static bool IsResetTokenExpired(this IRecoverable account, TimeSpan resetWithin)
        {
            return account.ResetPasswordSentAt.HasValue && 
                   DateTime.Now.Subtract(account.ResetPasswordSentAt.Value) > resetWithin;
        }
        
        public static void EnsureTokenIsValid(this IRecoverable account, TimeSpan resetWithin)
        {
            if (account.IsResetTokenExpired(resetWithin))
            {
                throw new ValidationException("Reset password token is expired");    
            }
        }
    }
}