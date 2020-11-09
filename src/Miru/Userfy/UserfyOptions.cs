using System;
using System.Threading.Tasks;
using Baseline.Dates;

namespace Miru.Userfy
{
    public class UserfyOptions
    {
        public string CookieName { get; set; }
        
        public string LoginPath { get; set; }

        public TimeSpan ResetPasswordWithin { get; set; } = 5.Hours();
        
        public TimeSpan RememberFor { get; set; } = 2.Weeks();
    }
}