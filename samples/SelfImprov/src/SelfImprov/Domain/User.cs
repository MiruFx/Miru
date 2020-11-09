using System;
using Miru.Domain;
using Miru.Userfy;

namespace SelfImprov.Domain
{
    public class User : Entity, IUser, ITimeStamped
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Display => Email;
    }
}