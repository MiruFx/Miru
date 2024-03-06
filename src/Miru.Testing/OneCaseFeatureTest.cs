namespace Miru.Testing;

public abstract class OneCaseFeatureTest : MiruTest, ICasePerFeatureTest
{
#pragma warning disable 1998
    public virtual async Task GivenAsync()
#pragma warning restore 1998
    {
    }
        
    public virtual void Given()
    {
    }

    [OneTimeSetUp]
    public async Task OneTimeSetupAsync() => await GivenAsync();
        
    [OneTimeSetUp]
    public void OneTimeSetup() => Given();
}