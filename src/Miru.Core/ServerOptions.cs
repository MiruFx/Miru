namespace Miru.Core;

public abstract class ServerOptions
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
}