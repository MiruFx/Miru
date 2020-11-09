using System;

namespace Miru.Userfy
{
    public interface IConfirmable
    {
        string ConfirmationToken { get; set; }
        DateTime? ConfirmedAt { get; set; }
        DateTime? ConfirmationSentAt { get; set; }
    }
    
    public static class ConfirmableExtensions
    {
        public static void ConfirmActivation(this IConfirmable confirmable)
        {
            confirmable.ConfirmationToken = null;
            confirmable.ConfirmationSentAt = null;
            confirmable.ConfirmedAt = DateTime.Now;
        }
    }
}