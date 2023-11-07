using System;

namespace Miru.Fabrication;

public class FabSupport
{
    public Fixture Fixture { get; }
        
    public Faker Faker { get; }
        
    public FabricatedSession Session { get; }
        
    internal IServiceProvider ServiceProvider { get; }
        
    public FabSupport(
        Fixture fixture,
        Faker faker,
        FabricatedSession session,
        IServiceProvider serviceProvider)
    {
        Fixture = fixture;
        Faker = faker;
        Session = session;
        ServiceProvider = serviceProvider;
    }
}