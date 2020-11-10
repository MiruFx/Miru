namespace Miru.Fabrication.FixtureConventions
{
    public static class AutoFakerConvention
    {
        public static ConventionExpression AddAutoFaker(this ConventionExpression cfg)
        {
            Names(cfg);
            Address(cfg);
            
            cfg.Use(f => f.Phone.PhoneNumber()).If(_ =>    
            {
                _.IfPropertyNameIs("Phone");
                _.IfPropertyNameIs("PhoneNumber");
            });
            
            cfg.IfPropertyNameIs("Email").Use(faker => faker.Internet.Email());

            cfg.IfPropertyNameIs("CompanyName").Use(faker => faker.Company.CompanyName());
            
            return cfg;
        }

        private static void Address(ConventionExpression cfg)
        {
            cfg.IfPropertyNameIs("Street").Use(faker => faker.Address.StreetAddress());

            cfg.IfPropertyNameIs("Complement").Use(faker => faker.Address.BuildingNumber());

            cfg.IfPropertyNameIs("State").Use(faker => faker.Address.State());

            cfg.IfPropertyNameIs("City").Use(faker => faker.Address.City());

            cfg.IfPropertyNameIs("PostalCode").Use(faker => faker.Address.ZipCode());
        }

        private static void Names(ConventionExpression cfg)
        {
            cfg.IfPropertyNameIs("Name").Use(faker => faker.Name.FullName());
            cfg.IfPropertyNameIs("FullName").Use(faker => faker.Name.FullName());

            cfg.IfPropertyNameIs("FirstName").Use(faker => faker.Name.FirstName());
            cfg.IfPropertyNameIs("LastName").Use(faker => faker.Name.LastName());
        }
    }
}