using System;
using Miru.Domain;
using Miru.Userfy;

namespace SelfImprov.Domain
{
    public class User : UserfyUser
    {
        public string Name { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string Display => Email;
    }
}