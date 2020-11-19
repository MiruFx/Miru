using System;
using System.Threading.Tasks;
using Miru;
using Miru.Consolables;
using Miru.Core;
using Miru.Databases.EntityFramework;
using Mong.Database;
using Mong.Domain;
using Oakton;

namespace Mong.Consolables
{
    [Description("Seed database", Name = "seed")]
    public class SeedConsolable : Consolable
    {
        private readonly MongDbContext _db;

        public SeedConsolable(MongDbContext db)
        {
            _db = db;
        }

        public override async Task ExecuteAsync()
        {
            Console2.White("Seeding Users... ");

            _db.Users.AddIfNotExists(m => m.Name == "Admin", new User
            {
                Name = "Admin",
                Email = "admin@admin.com",
                HashedPassword = Hash.Create("admin"),
                IsAdmin = true,
                ConfirmedAt = DateTime.Now
            });
            
            Console2.GreenLine("✅");

            Console2.White($"Seeding Providers... ");
            
            _db.Providers.AddIfNotExists(m => m.Name == "Four", new Provider
            {
                Name = "Four", 
                Amounts = "10,20,30,50"
            });
            
            _db.Providers.AddIfNotExists(m => m.Name == "Lemon", new Provider
            {
                Name = "Lemon", 
                Amounts = "15,25,50"
            });
            
            _db.Providers.AddIfNotExists(m => m.Name == "G-Mobile", new Provider
            {
                Name = "G-Mobile", 
                Amounts = "10,15,20,30"
            });

            Console2.GreenLine("✅");
            
            await _db.SaveChangesAsync();
        }
    }
}