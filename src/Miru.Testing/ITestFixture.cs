namespace Miru.Testing
{
    public interface ITestFixture
    {
        IMiruApp App { get; }
        
        T Get<T>();
        
        object Get(Type type);
    }
}