namespace Miru.Hosting;

public interface IAppWarmup
{
    void InitiateServices();

    Task InitializeAsync();
}